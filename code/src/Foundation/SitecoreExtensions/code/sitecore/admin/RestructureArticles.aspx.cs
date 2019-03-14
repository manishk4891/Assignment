namespace SaudiAramco.Foundation.SitecoreExtensions.sitecore.admin
{
    using SaudiAramco.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Globalization;
    using Sitecore.SecurityModel;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI.WebControls;

    public partial class RestructureArticles : System.Web.UI.Page
    {
        private readonly Database dbContext = Sitecore.Configuration.Factory.GetDatabase("master");

        public struct HomePageType
        {
            public static readonly ID ID = new ID("{4A8822CC-B418-4452-AB0F-8BB6DF2F2114}");
        }

        public struct SectionPageType
        {
            public static readonly ID ID = new ID("{8EE208F9-A6A6-41E2-88A0-C188737A178C}");
        }

        public struct NewsArticlePage
        {
            public static readonly ID ID = new ID("{E84CC91E-BCF9-4695-A0B8-A9DC20B0A9F4}");
        }
        public struct NewsHubPage
        {
            public static readonly ID ID = new ID("{C91F636F-00B4-4FD8-828C-3E34B15886B5}");
        }

        public struct NewsItem
        {
            public static readonly ID ID = new ID("{DE64592B-2C42-4CF2-89DB-021E1571D719}");

            public struct Fields
            {
                public static readonly ID PublishDate = new ID("{6C165CB3-C47F-4459-B7CF-BEFB306A0869}");
            }
        }

        public struct ArticleSection
        {
            public static readonly ID ID = new ID("{96E6F4F3-63D1-40CC-AF71-68E1BA0AF7E2}");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Item homeItem = dbContext.GetItem("{DAC24EDD-44FB-42EF-9ECD-1E8DAF706386}");

            if (homeItem.IsDerived(HomePageType.ID))
            {
                List<Item> sections = homeItem.Children.Where(c => c.TemplateID.Equals(SectionPageType.ID)).ToList();
                if (sections.Any())
                {
                    foreach (Item sectionItem in sections)
                    {
                        List<Item> newsHubItems = sectionItem.Children.Where(c => c.TemplateID.Equals(NewsHubPage.ID)).ToList();

                        if (newsHubItems.Any())
                        {
                            ProcessNewsHubItems(newsHubItems);
                        }
                    }
                }
            }
        }

        private void ProcessNewsHubItems(List<Item> newsHubItems)
        {
            foreach (Item newsHub in newsHubItems)
            {
                if (newsHub.HasChildren)
                {
                    List<Item> newsArticleItems = newsHub.Children.Where(c => c.TemplateID.Equals(NewsArticlePage.ID)
                                                    && c.FieldHasValue(NewsItem.Fields.PublishDate)).ToList();
                    if (newsArticleItems.Any())
                    {
                        ProcessArticleItems(newsHub, newsArticleItems);
                    }
                }
            }
        }

        private void ProcessArticleItems(Item newsHub, List<Item> newsArticleItems)
        {
            foreach (Item article in newsArticleItems)
            {
                var languages = dbContext.GetLanguages();
                foreach (Language language in languages)
                {
                    bool hasVersion = HasLanguageVersion(article, language.CultureInfo.TwoLetterISOLanguageName);
                    if (hasVersion)
                    {
                        var languageSpecificItem = dbContext.GetItem(article.ID, language);
                        if (languageSpecificItem != null && languageSpecificItem.Versions.Count > 0)
                        {
                            ValidateDateValue(newsHub, languageSpecificItem);
                        }
                    }
                }
            }
        }

        private void ValidateDateValue(Item newsHub, Item article)
        {
            var publishDateFieldValue = article.Fields[NewsItem.Fields.PublishDate].Value;
            DateTime publishDateValue;
            if (DateTime.TryParseExact(publishDateFieldValue, "yyyyMMdd'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out publishDateValue))
            {
                GenerateSectionAndMoveItem(newsHub, article, publishDateValue);
            }
            else if(DateTime.TryParseExact(publishDateFieldValue, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out publishDateValue))
            {
                GenerateSectionAndMoveItem(newsHub, article, publishDateValue);
            }
        }

        private void GenerateSectionAndMoveItem(Item newsHub, Item article, DateTime publishDateValue)
        {
            Item sectionItem = newsHub.Children[publishDateValue.Year.ToString()];
            if (newsHub.Children[publishDateValue.Year.ToString()] == null)
            {
                sectionItem = CreateChildItem(publishDateValue.Year.ToString(), newsHub);
                if (sectionItem != null)
                {
                    article.MoveTo(sectionItem);
                }
            }
            else
            {
                article.MoveTo(sectionItem);
            }
        }

        private bool HasLanguageVersion(Item item, string languageName)
        {
            var language = item.Languages.FirstOrDefault(l => l.Name == languageName);
            if (language != null)
            {
                var languageSpecificItem = dbContext.GetItem(item.ID, language);
                if (languageSpecificItem != null && languageSpecificItem.Versions.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private Item CreateChildItem(string itemName, Item parentItem)
        {
            using (new SecurityDisabler())
            {
                TemplateItem template = dbContext.GetTemplate(ArticleSection.ID);
                Item sectionItem = parentItem.Add(itemName, template);

                if (sectionItem != null)
                {
                    return sectionItem;
                }
            }
            return null;
        }
    }
}