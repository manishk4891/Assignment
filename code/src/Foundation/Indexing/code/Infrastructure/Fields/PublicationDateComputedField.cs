namespace Assignment.Foundation.Indexing.Infrastructure.Fields
{
    using Models;
    using Assignment.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using System;

    public class PublicationDateComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null)
            {
                return null;
            }
            var item = indexItem.Item;
            var publicationDate = GetPublicationDate(item);
            if (publicationDate.HasValue)
            {
                return publicationDate.Value;
            }
            return null;
        }

        private DateTime? GetPublicationDate(Item item)
        {
            DateTime? result = null;
            if (item != null)
            {
                var fieldNames = new string[] { "Publish Date", "PublicationDate", "PublishDate", "PublishedDate" };
                foreach (var field in fieldNames)
                {
                    if (!string.IsNullOrWhiteSpace(item[field]))
                    {
                        var dateField = (DateField)item.Fields[field];
                        result = dateField.DateTime;
                        break;
                    }
                }
            }
            return result;
        }
    }
}