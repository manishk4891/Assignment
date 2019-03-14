namespace Assignment.Feature.News.Controllers
{
    using Foundation.SitecoreExtensions.Repositories;
    using Newtonsoft.Json;
    using Assignment.Feature.News.Models;
    using Assignment.Feature.News.Repositories;
    using Assignment.Foundation.Alerts.Extensions;
    using Assignment.Foundation.Alerts.Models;
    using Assignment.Foundation.Dictionary.Repositories;
    using Sitecore;
    using Sitecore.Mvc.Presentation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Assignment.Foundation.SitecoreExtensions.Attributes;
    using Sitecore.Data.Items;
    using Assignment.Foundation.SitecoreExtensions.Extensions;

    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            this._newsRepository = newsRepository;
        }

        // Recent & Popular News Component
        public ActionResult GetNewsListing()
        {
            // If Datasource is not provided, then return and show the informative message to user and ask for setting datasource in rendering component.
            if (string.IsNullOrEmpty(RenderingContext.Current?.Rendering?.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ?
                    this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get(DictionaryPaths.NoDatasource, "This component does not have a datasource specified."),
                    InfoMessage.MessageType.Warning)) : null;
            }

            //Make a call to GetNewsItems() worker method which return model object
            return this.View("NewsListing", GetNewsItems());
        }

        private NewsRenderingModel GetNewsItems()
        {
            var model = new NewsRenderingModel();
            if (RenderingContext.Current != null)
            {

                model.Initialize(RenderingContext.Current.Rendering);
                var newsRoot = RenderingContext.Current.Rendering.Item;
                var renderingPropertiesRepository = new RenderingPropertiesRepository();

                //Get the rendering parameters value and cast them object of type NewsListingRenderingModel
                var parameters = renderingPropertiesRepository.Get<NewsListingRenderingModel>(RenderingContext.Current.Rendering);

                //If Rendering parameter "ArticleColumnValue" is not set with any value then set the default limit value as 4 
                var limit = parameters.ArticleColumnValue != 0 ? parameters.ArticleColumnValue : 4;
                var timeframeInDays = parameters.TimeFrame;
                var title = string.Empty;

                //Rendering parameter "NewsListingTypeValue" desides which type (Popular / Personalised / Recent) of News Listing to be presented. 
                switch (parameters.NewsListingTypeValue)
                {
                    case News.Constants.NewsListingTypes.Popular:
                        model.FeedType = "popular";
                        //Get Popular news from Analytics, like most visited News article items will be fetched.
                        model.Items = _newsRepository.GetPopularNews(newsRoot, timeframeInDays, limit).Select(x => _newsRepository.CreateNewsItem(x)).ToList();
                        title = DictionaryPhraseRepository.Current.Get(DictionaryPaths.PopularNewsTitle, "Popular News");
                        break;
                    case News.Constants.NewsListingTypes.Personalised:
                        // return popular items for personalised as default, personalised items (if applicable) will be loaded in with ajax on the front-end
                        model.FeedType = "personalised";
                        model.Items = _newsRepository.GetPopularNews(newsRoot, timeframeInDays, limit).Select(x => _newsRepository.CreateNewsItem(x)).ToList();
                        title = DictionaryPhraseRepository.Current.Get(DictionaryPaths.YourNewsTitle, "Your News");
                        break;
                    default:
                        model.FeedType = "recent";
                        //Get recently created news article items from search index
                        model.Items = _newsRepository.GetNews(newsRoot, null, 0, limit, null, DateTime.UtcNow).Select(x => _newsRepository.CreateNewsItem(x)).ToList();
                        title = DictionaryPhraseRepository.Current.Get(DictionaryPaths.RecentNewsTitle, "Recent News");
                        break;
                }
                var jsonSerializerSettings = new JsonSerializerSettings { Formatting = Formatting.None, NullValueHandling = NullValueHandling.Ignore };

                model.ApiUrl = "/" + Context.Language.CultureInfo.TwoLetterISOLanguageName + "/" + News.Constants.ApiUrls.News.Replace("{root}", newsRoot.ID.ToString()).Replace("{limit}", limit.ToString());
                model.ListingJson = JsonConvert.SerializeObject(GetJsonObject(model.Items, title, limit, model.ApiUrl, model.FeedType), jsonSerializerSettings);

            }
            return model;
        }

        [SkipAnalyticsTracking]
        [OutputCache(Duration = 300, VaryByParam = "root;limit;filters")]
        public ActionResult PersonalisedNews(string root, int limit, string filters)
        {
            // Ajax request for personalised news items (based on tags in users cookies)
            Item newsRoot = ItemExtensions.GetItem(root);
            if (limit != 4 && limit != 6) limit = 6;
            var tagIds = new string[0];
            if (!string.IsNullOrWhiteSpace(filters))
            {
                tagIds = filters.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim().ToLower()).ToArray();
            }
            var searchResults = _newsRepository.GetNews(newsRoot, tagIds, 0, limit).Select(x => _newsRepository.CreateNewsItem(x));
            var title = DictionaryPhraseRepository.Current.Get(DictionaryPaths.PersonalisedNewsTitle, "Personalised News");
            return new JsonResult { Data = GetJsonObject(searchResults, title, limit, null, null), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private object GetJsonObject(IEnumerable<NewsItem> items, string title, int limit, string endpoint, string feedtype)
        {
            // returns object in format required that can be serialized into json 
            string layoutName = "fourColumn";
            var newsSearchResults = items.Select(i => new { newsCard = new { time = i.LocationAndPublishDate, datetime = i.AttributePublishDate, title = i.NewsTitle, href = i.Url } });
            return new { endpoint = endpoint, feedType = feedtype, feedback = new { loadErrorMsg = DictionaryPhraseRepository.Current.Get(DictionaryPaths.AjaxError, "Cannot load data at this time"), loadingMsg = DictionaryPhraseRepository.Current.Get(DictionaryPaths.AjaxLoading, "Loading") }, heading = title, layout = layoutName, newsItems = newsSearchResults };
        }
    }
}