namespace Assignment.Feature.News.Repositories
{
    using Foundation.DependencyInjection;
    using Foundation.Indexing.Models;
    using Foundation.Indexing.Repositories;
    using Foundation.SitecoreExtensions.Extensions;
    using Models;
    using Sitecore.Analytics.Reporting;
    using Sitecore.Configuration;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Links;
    using Sitecore.Mvc.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    [Service(typeof(INewsRepository))]
    public class NewsRepository : INewsRepository
    {
        private readonly ISearchServiceRepository searchServiceRepository;
        protected ReportDataProviderBase ReportDataProvider = null;

        public NewsRepository(ISearchServiceRepository searchServiceRepository)
        {
            this.searchServiceRepository = searchServiceRepository;
        }

        private string _displayDateTimeFormat;
        public string DateTimeFormat {
            get
            {
                if (string.IsNullOrEmpty(_displayDateTimeFormat))
                {
                    _displayDateTimeFormat = ItemExtensions.GetSiteRootFieldValue(Foundation.Multisite.Templates.SiteSettings.Fields.DateFormat);
                }
                return _displayDateTimeFormat;
            }
        }

        //Method to get Popular / Personalised news articles
        public IEnumerable<Item> GetPopularNews(Item newsRoot, int timeFrameInDays, int limit)
        {
            var result = new List<Item>();
            var urlOptions = new UrlOptions() { LanguageEmbedding = LanguageEmbedding.Never, AlwaysIncludeServerUrl = false };
            var newsArticlesRoot = string.Format("'%{0}%'", newsRoot.Url(urlOptions));
            var popularNewsPageReport = GetPopularNewsReportingData(newsArticlesRoot, timeFrameInDays);
            if (popularNewsPageReport != null)
            {
                //These statements filter out the result by excluding context Item, and if context Item if of type NewsItem template, and take only number of records as per limit value.
                var excludedItemId = Sitecore.Context.Item?.ID;
                result = popularNewsPageReport.Rows.Cast<DataRow>().Select(row => row.Field<Guid>("itemid").ToString()).Distinct().Select(itemid => ItemExtensions.GetItem(itemid)).Where(x => x != null && x.ID != excludedItemId && x.IsDerived(Templates.NewsItem.ID)).Take(limit).ToList();
            }
            return result;
        }

        private DataTable GetPopularNewsReportingData(string newsRoot, int daysToAdd)
        {
            try
            {
                Log.Info($"NewsRepository - GetPopularNewsReportingData method Parameters - newsRoot = " + newsRoot + " | daysToAdd=" + daysToAdd, this);

                // Create ReportDataProviderBase factory using connection info from Sitecore.Analytics.Reporting.config
                var provider = (ReportDataProviderBase)Factory.CreateObject("reporting/dataProvider", false);

                // Load query from Sitecore ReportQuery item
                Item dataSourceItem = Sitecore.Context.Database.Items[Templates.NewsArticleReportQueryItem.ID];
                var dataSQLQuery = dataSourceItem.Fields["Query"].Value;

                Log.Info($"NewsRepository - GetPopularNewsReportingData method dataSQLQuery = " + dataSQLQuery, this);

                //set the parameter to query to filter records based on current context language, URL and start date.
                dataSQLQuery = dataSQLQuery.Replace("@Language", "'" + Sitecore.Context.Language.ToString() + "'");
                dataSQLQuery = dataSQLQuery.Replace("@LikeURL", newsRoot);
                dataSQLQuery = dataSQLQuery.Replace("@StartDate", daysToAdd.ToString());

                var query = new ReportDataQuery(dataSQLQuery);

                Log.Info($"NewsRepository - GetPopularNewsReportingData method query = " + query.Query, this);

                //Get data from reporting datasource in which query will be executed to get records.
                var response = provider.GetData("reporting", query, CachingPolicy.WithCacheDisabled);

                Log.Info($"NewsRepository - GetPopularNewsReportingData method Response Count = " + response.GetDataTable().Rows.Count, this);

                return response.GetDataTable();
            }
            catch (Exception ex)
            {
                Log.Error($"NewsRepository - GetPopularNewsReportingData method has error - {ex.Message}", ex, this);
            }
            return null;
        }

        public IEnumerable<Item> GetNews(Item newsRoot, string[] tagIds, int page, int limit)
        {
            return GetNews(newsRoot, tagIds, page, limit, null, null, null);
        }

        public IEnumerable<Item> GetNews(Item newsRoot, string[] tagIds, int page, int limit, DateTime? pubStart, DateTime? pubEnd)
        {
            return GetNews(newsRoot, tagIds, page, limit, pubStart, pubEnd, null);
        }

        //Get news article items from search index
        public IEnumerable<Item> GetNews(Item newsRoot, string[] tagIds, int page, int limit, DateTime? pubStart, DateTime? pubEnd, IEnumerable<Item> excludedItems)
        {
            if (newsRoot == null)
            {
                throw new ArgumentNullException(nameof(newsRoot));
            }
            var searchSettings = new SearchSettingsBase
            {
                Templates = new[] { Templates.NewsItem.ID },
                Root = newsRoot
            };
            var searchService = this.searchServiceRepository.Get(searchSettings);

            // sort by publication date in descending order
            var sort = new Dictionary<string, bool>();
            sort.Add(Foundation.Indexing.Constants.IndexFields.PublicationDate, true);
            if (excludedItems != null) limit += excludedItems.Count();

            var searchQuery = new SearchQuery() { Page = page, NoOfResults = limit, Sort = sort };

            // filter by tag (if specified)
            if (tagIds != null && tagIds.Length > 0)
            {
                var facets = new Dictionary<string, string[]>();
                facets.Add(Foundation.Indexing.Constants.Facets.Tags, tagIds);
                searchQuery.Facets = facets;
            }

            // filter by publication date
            searchQuery.PublicationDateStart = pubStart;
            searchQuery.PublicationDateEnd = pubEnd;

            var searchResponse = searchService.Search(searchQuery);
            var searchItems = searchResponse.Results.Select(x => x.Item).Where(x => x != null);
            
            // remove any excluded items from results
            if (excludedItems != null && excludedItems.Any())
            {
                var excludedIds = excludedItems.Select(x => x.ID);
                var filteredItems = searchItems.Where(x => !excludedIds.Contains(x.ID));
                if (filteredItems.Count() != searchItems.Count())
                {
                    searchItems = filteredItems;
                }
                searchItems = searchItems.Take(limit);
            }
            return searchItems;
        } 
        
        public NewsItem CreateNewsItem(Item item)
        {
            var newsItem = new NewsItem()
            {
                Item = item,
                Url = item.Url(),
                NewsTitle = item.Fields[Templates.NewsItem.Fields.NewsTitle].Value,
                Description = item.Fields[Templates.NewsItem.Fields.Description].Value,
                PublishDate = ((DateField)item.Fields[Templates.NewsItem.Fields.PublishDate]).DateTime,
                IsHighlightNews = item.Fields[Templates.NewsItem.Fields.IsHighlightNews].IsChecked(),
                Location = item.Fields[Templates.NewsItem.Fields.Location].Value
            };
            newsItem.DisplayPublishDate = newsItem.PublishDate.GetDateByLocal(this.DateTimeFormat);

            //Convert the month value of PublishDate to particular format for Arabic (ar) language.
            string month = newsItem.PublishDate.ToString("MMM");
            if (Sitecore.Context.Language.ToString().Equals("ar"))
            {
                month = FieldExtensions.GetDateByLocal(newsItem.PublishDate, "MMMM");
            }
            newsItem.DisplayPublishMonth = month;
            newsItem.AttributePublishDate = newsItem.PublishDate.ToString(Foundation.SitecoreExtensions.Constants.DateTimeAttributeValue.Format);
            newsItem.LocationAndPublishDate = !string.IsNullOrEmpty(newsItem.Location) ? $"{newsItem.Location}, {newsItem.DisplayPublishDate}" : newsItem.DisplayPublishDate;
            return newsItem;
        }      
    }
}
