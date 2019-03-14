namespace Assignment.Foundation.Indexing.Models
{
    using System;
    using Sitecore.Data.Items;
    using Assignment.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Data.Fields;

    public class SearchResult : ISearchResult
    {
        private Uri _url;

        public SearchResult(Item item)
        {
            this.Item = item;
        }

        public Item Item { get; }
        public ImageField Image { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Section { get; set; }
        public string Description { get; set; }

        public Uri Url
        {
            get
            {
                return this._url ?? new Uri(this.Item.Url(), UriKind.Relative);
            }
            set
            {
                this._url = value;
            }
        }

        public string ViewName { get; set; }
    }
}