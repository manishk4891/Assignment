namespace Assignment.Feature.News
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("news", Constants.ApiUrls.News, new { controller = "News", action = "PersonalisedNews" });
        }
    }
}