namespace Assignment.Foundation.Indexing.Infrastructure.Fields
{
    using System.Collections.Generic;
    using Sitecore;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data.Items;
    using Assignment.Foundation.SitecoreExtensions.Extensions;
    using System.Linq;

    public class AllTagsComputedField : IComputedIndexField
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

            var tags = this.GetAllTags(item);

            return tags;
        }

        public List<string> GetAllTags(Item item)
        {
            var result = new List<string>();
            if (item != null && item.FieldHasValue(SitecoreExtensions.Templates.Tagging.Fields.Tags))
            {
                result = item.GetMultiListValueItemsOfType(SitecoreExtensions.Templates.Tagging.Fields.Tags, SitecoreExtensions.Templates.Tag.ID, true).Select(x => IdHelper.NormalizeGuid(x.ID)).ToList();
            }
            return result;
        }
    }
}