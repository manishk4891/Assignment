namespace Assignment.Foundation.Indexing.Infrastructure.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.ContentSearch.Linq.Common;
    using Sitecore.ContentSearch.Linq.Methods;
    using Sitecore.ContentSearch.Pipelines.GetFacets;
    using Sitecore.ContentSearch.Pipelines.ProcessFacets;
    using Assignment.Foundation.Indexing.Infrastructure.Providers;
    using Assignment.Foundation.Indexing.Models;
    using Assignment.Foundation.Indexing.Repositories;
    using Assignment.Foundation.SitecoreExtensions.Extensions;

    public class ContentItemTypeComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexItem = indexable as SitecoreIndexableItem;
            var item = indexItem?.Item;
            if (item == null)
            {
                return null;
            }

            var formatter = IndexingProviderRepository.SearchResultFormatters.FirstOrDefault(provider => provider.SupportedTemplates.Any(item.IsDerived));
            if (formatter == null || formatter is FallbackSearchResultFormatter)
            {
                return null;
            }
            return formatter.ContentType;
        }
    }
}