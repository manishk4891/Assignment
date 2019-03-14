namespace Assignment.Feature.News
{
    public struct Constants
    {
        public struct NewsListingTypes
        {
            public const string Recent = "Recent";
            public const string Popular = "Popular";
            public const string Personalised = "Personalised";
        }

        public struct ApiUrls
        {
            public const string News = "api/feature/news/{root}/{limit}";
        }
    }
}