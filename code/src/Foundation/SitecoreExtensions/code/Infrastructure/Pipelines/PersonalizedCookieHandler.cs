namespace Assignment.Foundation.SitecoreExtensions.Infrastructure.Pipelines
{
    using Sitecore.Pipelines.HttpRequest;
    using System;
    using System.Web;

    public class PersonalizedCookieHandler : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if ((!Sitecore.Context.PageMode.IsPreview || !Sitecore.Context.PageMode.IsExperienceEditor)
                && (Sitecore.Context.Item != null && Sitecore.Context.Item.Fields[Templates.Tagging.Fields.Tags] != null && !string.IsNullOrEmpty(Sitecore.Context.Item.Fields[Templates.Tagging.Fields.Tags].Value)))
            {
                if (HttpContext.Current.Request.Cookies[Constants.PersonalizedTagsCookieName] != null
                && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[Constants.PersonalizedTagsCookieName].Value))
                {
                    HttpContext.Current.Response.Cookies.Remove(Constants.PersonalizedTagsCookieName);
                }

                string tags = Sitecore.Context.Item.Fields[Templates.Tagging.Fields.Tags].Value;
                var c = new HttpCookie(Constants.PersonalizedTagsCookieName);
                c.Value = tags;
                c.Expires = DateTime.MaxValue;
                HttpContext.Current.Response.Cookies.Add(c);
            }
        }
    }
}