using System.Collections.Generic;
using Sitecore.Mvc.Presentation;

namespace Assignment.Feature.News.Models
{
    public class NewsRenderingModel : RenderingModel
    {
        public string ApiUrl { get; set; }
        public string FeedType { get; set; }
        public string ListingJson { get; set; }
        public IList<NewsItem> Items { set; get; }      
    }
}