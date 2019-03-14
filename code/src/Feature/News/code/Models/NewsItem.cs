using Sitecore.Data.Items;
using System;
using System.Collections.Generic;

namespace Assignment.Feature.News.Models
{
    public class NewsItem
    {
        public Item Item { get; set; }
        public string Url { get; set; }
        public string NewsTitle { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string DisplayPublishDate { get; set; }
        public string DisplayPublishMonth { get; set; }
        public string AttributePublishDate { get; set; }
        public bool IsHighlightNews { get; set; }
        public string Location { set; get; }
        public string LocationAndPublishDate { get; set; }
    }

}