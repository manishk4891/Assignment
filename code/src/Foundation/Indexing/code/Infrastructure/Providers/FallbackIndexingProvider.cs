namespace Assignment.Foundation.Indexing.Infrastructure.Providers
{
    using System.Collections.Generic;
    using System.Configuration.Provider;
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.Data;
    using Models;

    public class FallbackSearchResultFormatter : ProviderBase, ISearchResultFormatter
    {
        public string ContentType
        {
            get
            {
                return "[Unknown]";
            }
        }

        public IEnumerable<ID> SupportedTemplates
        {
            get
            {
                return new ID[0];
            }
        }

        public void FormatResult(SearchResultItem item, ISearchResult formattedResult)
        {
            formattedResult.Title = $"[{item.Name}]";
            formattedResult.Description = $"[This item is indexed but has no content provider: {item.Path}]";
        }

    }
}