namespace Assignment.Feature.News.Models
{
    using Assignment.Foundation.Multisite;
    using Assignment.Foundation.SitecoreExtensions.Extensions;
    using System.Web;

    public class NewsListingRenderingModel
    {
        public string NewsArticleCount { get; set; }
        public int TimeFrame { get; set; }
        public string NewsListingType { get; set; }

        public int ArticleColumnValue
        {
            get
            {
                int newsItemCount = 0;
                if (NewsArticleCount != null)
                {
                    int.TryParse(ItemExtensions.GetItem(HttpUtility.UrlDecode(this.NewsArticleCount)).Fields[Templates.KeyValue.Fields.Value].Value, out newsItemCount);
                }
                return newsItemCount;
            }
        }

        public string NewsListingTypeValue
        {
            get
            {
                return ItemExtensions.GetItem(HttpUtility.UrlDecode(this.NewsListingType)).Fields[Templates.KeyValue.Fields.Value].Value;
            }
        }
    }
}