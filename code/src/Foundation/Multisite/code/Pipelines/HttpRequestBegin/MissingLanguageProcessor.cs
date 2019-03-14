namespace Assignment.Foundation.Multisite.Pipelines.HttpRequestBegin
{
    using Assignment.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Links;
    using Sitecore.Pipelines.HttpRequest;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Web;

    public class MissingLanguageProcessor : HttpRequestBase
    {
        protected override void Execute(HttpRequestArgs args)
        {
            if (Sitecore.Context.Item != null && !Sitecore.Context.Item.HasContextLanguage())
            {
                RedirectToParent(args, Sitecore.Context.Item.Parent);
            }
        }

        private void RedirectToParent(HttpRequestArgs args, Item parent)
        {
            // if no parent or parent is homepage, redirect to homepage.
            if (parent == null || parent.IsDerived(Templates.HomePageType.ID))
            {
                RedirectToHomePage(args);
            }
            else
            { 
                if (parent.HasContextLanguage() && parent.HasLayout())
                {
                    Sitecore.Context.Item = parent;
                    args.Context.Response.Redirect(Sitecore.Context.Item.Url());
                }
                else
                {
                    RedirectToParent(args, parent.Parent);
                }
            }
        }

        private static void RedirectToHomePage(HttpRequestArgs args)
        {
            var siteStartItem = SiteExtensions.GetStartItem(Sitecore.Context.Site);
            Sitecore.Context.Item = siteStartItem;
            args.Context.Response.Redirect(Sitecore.Context.Item.Url());
        }
    }
}