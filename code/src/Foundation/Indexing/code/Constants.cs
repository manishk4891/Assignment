namespace Assignment.Foundation.Indexing
{
    public struct Constants
    {
        public struct IndexFields
        {
            public const string HasPresentation = "has_presentation";
            public const string AllTemplates = "all_templates";
            public const string IsLatestVersion = "_latestversion";
            public const string HasSearchResultFormatter = "has_search_result_formatter";
            public const string ContentType = "content_type";
            public const string SearchResultFormatter = "search_result_formatter";
            public const string SiteSection = "site_section";
            public const string PublicationDate = "pubdate";
            public const string Score = "score";
            public const string Tags = "all_tags";
            public const string FullPath = "_fullpath";
        }

        public struct Facets
        {
            public const string Tags = "all_tags_sm";
            public const string ContentType = "content_item_type_s";
            public const string SiteSection = "site_section_s";
            public const string PublicationCategory = "publication_category_s";
        }
    }
}