namespace Assignment.Feature.News.Comparers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sitecore.Data.Comparers;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using System.Globalization;

    public class PublishDateComparer : ExtractedKeysComparer
    {
        protected override int DoCompare(Item item1, Item item2)
        {
            return this.GetPublishDate(item2).CompareTo(this.GetPublishDate(item1));
        }
        private DateTime GetPublishDate(Item item)
        {
            DateTime dateTime = DateTime.MaxValue;
            foreach (Item obj in item.Versions.GetVersions(true))
            {
                try
                {
                    var publishDateFieldValue = obj.Fields[Templates.NewsItem.Fields.PublishDate].Value;
                    if (!string.IsNullOrEmpty(publishDateFieldValue))
                    {
                        DateTime publishDateValue = DateTime.ParseExact(publishDateFieldValue, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture);

                        if (publishDateValue != DateTime.MinValue && publishDateValue.CompareTo(dateTime) < 0)
                            dateTime = publishDateValue;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"PublishDateComparer -> GetPublishDate: Error : " + ex.Message + " | Item: " + obj.ID, this);
                }
            }
            if (dateTime != DateTime.MaxValue)
                return dateTime;

            return DateTime.MinValue;
        }

        public override IKey ExtractKey(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            KeyObj keyObj = new KeyObj()
            {
                Item = item,
                Key = this.GetPublishDate(item)
            };
            return keyObj;
        }

        protected override int CompareKeys(IKey key1, IKey key2)
        {
            Assert.ArgumentNotNull(key1, "key1");
            Assert.ArgumentNotNull(key2, "key2");
            return ((DateTime)key2.Key).CompareTo((DateTime)key1.Key);
        }
    }
}


