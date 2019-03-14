using System;
using System.Net;
using System.Web;
using Sitecore.Configuration;
using Sitecore.Pipelines.HttpRequest;
using Assignment.Foundation.Multisite.Loggers;
using Sitecore.Diagnostics;
using Sitecore;

namespace Assignment.Foundation.Multisite.Pipelines.HttpRequestEnd
{
    public class SetPageCacheHeaders : HttpRequestBase
    {
        protected override void Execute(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (HttpContext.Current.Response.StatusCode != 200 || Context.Site == null || Context.Item == null || Context.Item.Visualization.Layout == null)
            {
                return;
            }
            Profiler.StartOperation("Setting cache-control headers for \"" + Context.Item.Name + "\".");
            var maxAge = Settings.GetTimeSpanSetting("Foundation.Multisite.PageCacheHeaderMaxAge", new TimeSpan(0, 1, 0));
            if (maxAge.TotalSeconds > 0)
            {
                var modifiedDate = DateTime.Now;
                var expiryDate = modifiedDate.Add(maxAge);
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
                HttpContext.Current.Response.Cache.SetMaxAge(maxAge);
                HttpContext.Current.Response.Cache.SetLastModified(modifiedDate);
                HttpContext.Current.Response.Cache.SetExpires(expiryDate);
            }
            Profiler.EndOperation();
        }
    }
}