namespace Assignment.Foundation.Indexing
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct IndexedItem
        {
            public static readonly ID ID = new ID("{8FD6C8B6-A9A4-4322-947E-90CE3D94916D}");

            public struct Fields
            {
                public static readonly ID IncludeInSearchResults = new ID("{8D5C486E-A0E3-4DBE-9A4A-CDFF93594BDA}");
                public const string IncludeInSearchResults_FieldName = "IncludeInSearchResults";
            }
        }
        public struct SectionPageType
        {
            public static readonly ID ID = new ID("{8EE208F9-A6A6-41E2-88A0-C188737A178C}");
        }
    }
}