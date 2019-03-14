namespace Assignment.Foundation.SitecoreExtensions
{
    public struct Constants
    {
        public const string PersonalizedTagsCookieName = "Personalized_Tags";
        public struct DynamicPlaceholdersLayoutParameters
        {
            public static string UseStaticPlaceholderNames => "UseStaticPlaceholderNames";
        }

        public struct ImageRenditions
        {
            public static int Thumbnail => 480;
            public static int Small => 680;
            public static int Medium => 1024;
            public static int Large => 1200;
            public static int XLarge => 1440;
            public static int RetinaThumbnail => 960;
            public static int RetinaSmall => 1360;
            public static int RetinaMedium => 2048;
            public static int RetinaLarge => 2400;
            public static int RetinaExtraLarge => 2880;


        }

        public struct DateTimeAttributeValue
        {
            public static string Format => "yyyy-MM-dd";
        }
    }
}






