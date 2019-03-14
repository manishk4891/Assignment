namespace Assignment.Foundation.SitecoreExtensions
{
    using Sitecore.Data;
    public struct Templates
    {
        public struct Tagging
        {
            public static readonly ID ID = new ID("{6FF2C5BC-0D94-4D54-B75B-A9B526D3F978}");

            public struct Fields
            {
                public static readonly ID Tags = new ID("{9A690038-2B7D-4D3A-B001-FCA2FADAB6CD}");
            }
        }

        public struct Tag
        {
            public static readonly ID ID = new ID("{68BA23FD-8270-4675-97EA-4FAFC7CF3AB9}");
        }

        public struct RelatedNewsRendering
        {
            public static readonly ID ID = new ID("{534FFC3F-26BD-4D42-BA0A-DBCB56C98833}");
        }
    }
}