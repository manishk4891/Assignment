namespace Assignment.Foundation.Multisite
{
    using Sitecore.Data;
    using Sitecore.Shell.Framework.Commands.Masters;


    public class Templates
    {
        public struct Site
        {
            public static readonly ID ID = new ID("{BB85C5C2-9F87-48CE-8012-AF67CF4F765D}");
        }

        public struct DatasourceConfiguration
        {
            public static readonly ID ID = new ID("{C82DC5FF-09EF-4403-96D3-3CAF377B8C5B}");

            public struct Fields
            {
                public static readonly ID DatasourceLocation = new ID("{5FE1CC43-F86C-459C-A379-CD75950D85AF}");
                public static readonly ID DatasourceTemplate = new ID("{498DD5B6-7DAE-44A7-9213-1D32596AD14F}");
            }
        }

        public struct SiteSettings
        {
            public static readonly ID ID = new ID("{BCCFEBEA-DCCB-48FE-9570-6503829EC03F}");
            public struct Fields
            {
                public static readonly ID DateFormat = new ID("{B907191A-074C-47C1-8E3C-E07D36C6CC0D}");
            }
        }

        public struct RenderingOptions
        {
            public static readonly ID ID = new ID("{D1592226-3898-4CE2-B190-090FD5F84A4C}");

            public struct Fields
            {
                public static readonly ID DatasourceLocation = new ID("{B5B27AF1-25EF-405C-87CE-369B3A004016}");
                public static readonly ID DatasourceTemplate = new ID("{1A7C85E5-DC0B-490D-9187-BB1DBCB4C72F}");
            }
        }

        public struct KeyValue
        {
            public static readonly ID ID = new ID("{5E402071-636E-43EC-AF5B-65A2C410D86E}");

            public struct Fields
            {
                public static readonly ID Key = new ID("{61CB5E3F-0868-424E-81A5-36A0D51A8909}");
                public static readonly ID Value = new ID("{D7CD624F-AF8F-4E8D-9D85-85E4B662CBA8}");                
            }
        }

        public struct SectionPageType
        {
            public static readonly ID ID = new ID("{8EE208F9-A6A6-41E2-88A0-C188737A178C}");
        }
        public struct HomePageType
        {
            public static readonly ID ID = new ID("{4A8822CC-B418-4452-AB0F-8BB6DF2F2114}");
        }
    }
}