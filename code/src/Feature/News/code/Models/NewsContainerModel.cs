using System.Collections.Generic;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;

namespace Assignment.Feature.News.Models
{
    public class NewsContainerModel : RenderingModel
    {
        public IList<NewsItem> Items { set; get; } = new List<NewsItem>();
        public string SearchResultPage { get; set; }
    }
}