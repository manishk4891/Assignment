namespace Assignment.Feature.News
{
    using Sitecore.Data;
    public struct Templates
    {
        public struct NewsItem
        {
            public static readonly ID ID = new ID("{DE64592B-2C42-4CF2-89DB-021E1571D719}");

            public struct Fields
            {
                public static readonly ID NewsTitle = new ID("{0CE79CEB-465D-40E9-BFF8-83C7EDEDF943}");
                public static readonly ID Description = new ID("{5704C181-AFBE-4213-86E6-E09EB2DFCBB8}");
                public static readonly ID Image = new ID("{2BC079FF-7EE7-41D4-9C63-36487CE07FFD}");
                public static readonly ID PublishDate = new ID("{6C165CB3-C47F-4459-B7CF-BEFB306A0869}");
                public static readonly ID IsHighlightNews = new ID("{B345A30A-673C-41C7-BC4D-B0FD2F3A761F}");
                public static readonly ID Tags = new ID("{9A690038-2B7D-4D3A-B001-FCA2FADAB6CD}");
                public static readonly ID LocationTag = new ID("{08E29657-7935-4117-A816-BD130345D2D4}");
                public static readonly ID Location = new ID("{D32BEB16-824F-4A15-9D82-3709AAE285BD}");

                public static readonly ID Caption = new ID("{9F1EB29A-2D3A-4B72-B499-123C7DA296F5}");
            }
        }

        public struct RenderingParameter
        {
            public static readonly ID ID = new ID("{82F942F0-8D8C-4A21-A8FA-C398D810E004}");

            public struct Fields
            {
                public static readonly ID Limit = new ID("{C36F34ED-08BB-4B8B-B741-608611804E97}");
                public static readonly string LimitParameter = "Limit";
                public static readonly ID Tag = new ID("{EF0098E6-7375-44C1-A75D-BC293F448940}");
                public static readonly string TagParameter = "Tag";
            }
        }

        public struct NewsListingParameters
        {
            public static readonly ID ID = new ID("{0096186A-BE4A-45B3-8209-15F465C6FAF3}");

            public struct Fields
            {
                public static readonly ID NewsListingType = new ID("{E2AE1E94-836A-4702-AF0F-8C1D61C58334}");
                public static readonly string NewsListingType_FieldName = "NewsListingType";

                public static readonly ID NewsArticleCount = new ID("{0935EB8D-B4E2-424E-9956-B116890FB12A}");
                public static readonly string NewsArticleCount_FieldNam = "NewsArticleCount";

                public static readonly ID TimeFrame = new ID("{30834FF2-A9BF-4771-83E9-61070D764821}");
                public static readonly string TimeFrame_FieldName = "TimeFrame";

            }

        }

        public struct NewsArticleReportQueryItem
        {
            public static readonly ID ID = new ID("{3ED36210-6A40-4F12-8409-EF47C335ED8B}");
        }

        public struct NewsArticlePage
        {
            public static readonly ID ID = new ID("{E84CC91E-BCF9-4695-A0B8-A9DC20B0A9F4}");
        }
        public struct NewsHubPage
        {
            public static readonly ID ID = new ID("{C91F636F-00B4-4FD8-828C-3E34B15886B5}");
        }

        public struct RelatedNewsContainer
        {
            public struct Fields
            {
                public static readonly ID Title = new ID("{111FC737-06CE-40D0-A22A-224F3BD86AFE}");
            }
        }

        public struct NewsGlobalSettings
        {
            public static readonly ID TemplateID = new ID("{0BD26895-D30A-4157-8C3E-7599824A1D89}");
            public struct Fields
            {
                public static readonly ID NewsArchivePage = new ID("{65B38033-3F89-4E20-908D-F0B6AA131F7B}");
                public static readonly ID NewsHubPage = new ID("{B7BC45D8-7E9E-4A7B-B1A7-7E706C47AEDC}");
            }
        }

        public struct ArticleSection
        {
            public static readonly ID ID = new ID("{96E6F4F3-63D1-40CC-AF71-68E1BA0AF7E2}");
        }
    }
}