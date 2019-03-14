namespace Assignment.Foundation.Indexing.Infrastructure.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Data.Items;
    using Assignment.Foundation.SitecoreExtensions.Extensions;
    using Models;
    using Sitecore.ContentSearch.Utilities;

    public class SiteSectionComputedField : IComputedIndexField
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
            var siteSectionItem = GetSiteSectionItem(item);
            if (siteSectionItem != null)
            {
                return IdHelper.NormalizeGuid(siteSectionItem.ID);
            }
            return null;
        }

        private Item GetSiteSectionItem(Item item)
        {
            if (item != null)
            {
                return item.GetAncestorOrSelfOfTemplate(Templates.SectionPageType.ID);
            }
            return null;
        }
    }
}