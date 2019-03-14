using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment.Feature.News
{
    public struct DictionaryPaths
    {
        public static readonly string RecentNewsTitle = "/News/RecentNewsTitle";
        public static readonly string PopularNewsTitle = "/News/PopularNewsTitle";
        public static readonly string PersonalisedNewsTitle = "/News/PersonalisedNewsTitle";
        public static readonly string NoArticles = "/News/NoArticles";
        public static readonly string NoCookieInPreviewMode = "/News/NoCookieInPreviewMode";
        public static readonly string YourNewsTitle = "/News/YourNewsTitle";
        public static readonly string NoDatasource = "/News/NoDatasource";
        public static readonly string AjaxLoading = "/News/AjaxLoading";
        public static readonly string AjaxError = "/News/AjaxError";
    }
}