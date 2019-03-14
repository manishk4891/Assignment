namespace Assignment.Foundation.SitecoreExtensions.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Data.Managers;
    using Sitecore.Diagnostics;
    using Assignment.Foundation.SitecoreExtensions.Services;
    using Sitecore.Links;
    using Sitecore.Resources.Media;
    using Sitecore;

    public static class ItemExtensions
    {
        public static string Url(this Item item, UrlOptions options = null)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (options != null)
                return LinkManager.GetItemUrl(item, options);
            return !item.Paths.IsMediaItem ? LinkManager.GetItemUrl(item) : MediaManager.GetMediaUrl(item);
        }

        public static string ImageUrl(this Item item, ID imageFieldId, MediaUrlOptions options = null)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var imageField = (ImageField)item.Fields[imageFieldId];
            return imageField?.MediaItem == null ? string.Empty : imageField.ImageUrl(options);
        }

        public static string ImageUrl(this MediaItem mediaItem, int width, int height)
        {
            if (mediaItem == null)
                throw new ArgumentNullException(nameof(mediaItem));

            var options = new MediaUrlOptions { Height = height, Width = width };
            var url = MediaManager.GetMediaUrl(mediaItem, options);
            var cleanUrl = StringUtil.EnsurePrefix('/', url);
            var hashedUrl = HashingUtils.ProtectAssetUrl(cleanUrl);

            return hashedUrl;
        }


        public static Item TargetItem(this Item item, ID linkFieldId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (item.Fields[linkFieldId] == null || !item.Fields[linkFieldId].HasValue)
                return null;
            return ((LinkField)item.Fields[linkFieldId]).TargetItem ?? ((ReferenceField)item.Fields[linkFieldId]).TargetItem;
        }

        public static string MediaUrl(this Item item, ID mediaFieldId, MediaUrlOptions options = null)
        {
            var targetItem = item.TargetItem(mediaFieldId);
            return targetItem == null ? string.Empty : (MediaManager.GetMediaUrl(targetItem) ?? string.Empty);
        }


        public static bool IsImage(this Item item)
        {
            return new MediaItem(item).MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsVideo(this Item item)
        {
            return new MediaItem(item).MimeType.StartsWith("video/", StringComparison.InvariantCultureIgnoreCase);
        }

        public static Item GetAncestorOrSelfOfTemplate(this Item item, ID templateID)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return item.IsDerived(templateID) ? item : item.Axes.GetAncestors().LastOrDefault(i => i.IsDerived(templateID));
        }

        public static IList<Item> GetAncestorsAndSelfOfTemplate(this Item item, ID templateID)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var returnValue = new List<Item>();
            if (item.IsDerived(templateID))
                returnValue.Add(item);

            returnValue.AddRange(item.Axes.GetAncestors().Reverse().Where(i => i.IsDerived(templateID)));
            return returnValue;
        }

        public static string LinkFieldUrl(this Item item, ID fieldID)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (ID.IsNullOrEmpty(fieldID))
            {
                throw new ArgumentNullException(nameof(fieldID));
            }
            var field = item.Fields[fieldID];
            if (field == null || !(FieldTypeManager.GetField(field) is LinkField))
            {
                return string.Empty;
            }
            else
            {
                LinkField linkField = (LinkField)field;
                return GetLinkFieldUrl(linkField);
            }
        }

        public static string GetLinkFieldUrl(LinkField linkField)
        {
            switch (linkField.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return linkField.TargetItem != null ? Sitecore.Links.LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return linkField.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(linkField.TargetItem) : string.Empty;
                case "external":
                    // Just return external links
                    return linkField.Url;
                case "anchor":
                    // Prefix anchor link with # if link if not empty
                    return !string.IsNullOrEmpty(linkField.Anchor) ? "#" + linkField.Anchor : string.Empty;
                case "mailto":
                    // Just return mailto link
                    return linkField.Url;
                case "javascript":
                    // Just return javascript
                    return linkField.Url;
                default:
                    // Just please the compiler, this
                    // condition will never be met
                    return linkField.Url;
            }
        }

        public static string LinkFieldTarget(this Item item, ID fieldID)
        {
            return item.LinkFieldOptions(fieldID, LinkFieldOption.Target);
        }

        public static string LinkFieldOptions(this Item item, ID fieldID, LinkFieldOption option)
        {
            XmlField field = item.Fields[fieldID];
            switch (option)
            {
                case LinkFieldOption.Text:
                    return field?.GetAttribute("text");
                case LinkFieldOption.LinkType:
                    return field?.GetAttribute("linktype");
                case LinkFieldOption.Class:
                    return field?.GetAttribute("class");
                case LinkFieldOption.Alt:
                    return field?.GetAttribute("title");
                case LinkFieldOption.Target:
                    return field?.GetAttribute("target");
                case LinkFieldOption.QueryString:
                    return field?.GetAttribute("querystring");
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, null);
            }
        }

        public static string GetDropTreeFieldUrl(this Item item, ID fieldID)
        {
            string Url;
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (ID.IsNullOrEmpty(fieldID))
            {
                throw new ArgumentNullException(nameof(fieldID));
            }
            var field = item.Fields[fieldID];
            if (field == null || !(FieldTypeManager.GetField(field) is ReferenceField))
            {
                Url = string.Empty;
            }
            else
            {
                ReferenceField referenceField = (ReferenceField)field;
                Url = referenceField.Value != string.Empty ? GetItem(referenceField.Value).Url() : string.Empty;
            }
            return Url;
        }

        public static bool HasLayout(this Item item)
        {
            return item?.Visualization?.Layout != null;
        }


        public static bool IsDerived(this Item item, ID templateId)
        {
            if (item == null)
                return false;

            return !templateId.IsNull && item.IsDerived(item.Database.Templates[templateId]);
        }

        private static bool IsDerived(this Item item, Item templateItem)
        {
            if (item == null)
                return false;

            if (templateItem == null)
                return false;

            var itemTemplate = TemplateManager.GetTemplate(item);
            return itemTemplate != null && (itemTemplate.ID == templateItem.ID || itemTemplate.DescendsFrom(templateItem.ID));
        }

        public static bool FieldHasValue(this Item item, ID fieldID)
        {
            return item.Fields[fieldID] != null && !string.IsNullOrWhiteSpace(item.Fields[fieldID].Value);
        }

        public static int? GetInteger(this Item item, ID fieldId)
        {
            int result;
            return !int.TryParse(item.Fields[fieldId].Value, out result) ? new int?() : result;
        }

        public static IEnumerable<Item> GetMultiListValueItems(this Item item, ID fieldId)
        {
            return new MultilistField(item.Fields[fieldId]).GetItems();
        }

        public static IEnumerable<Item> GetMultiListValueItemsOfType(this Item item, ID fieldId, ID templateId, bool includeAncestors)
        {
            var result = new MultilistField(item.Fields[fieldId]).GetItems().Where(x => x.IsDerived(templateId)).ToList();
            if (includeAncestors)
            {
                var ancestors = new List<Item>();
                foreach (var valueItem in result)
                {
                    foreach (var ancestor in valueItem.Axes.GetAncestors().Reverse())
                    {
                        if (!ancestor.IsDerived(templateId))
                        {
                            break;
                        }
                        if (!result.Exists(x => x.ID == ancestor.ID) && !ancestors.Exists(x => x.ID == ancestor.ID))
                        {
                            ancestors.Add(ancestor);
                        }
                    }
                }
                result.AddRange(ancestors);
            }
            return result;
        }

        public static bool HasContextLanguage(this Item item)
        {
            var latestVersion = item.Versions.GetLatestVersion();
            return latestVersion?.Versions.Count > 0;
        }

        public static HtmlString Field(this Item item, ID fieldId)
        {
            Assert.IsNotNull(item, "Item cannot be null");
            Assert.IsNotNull(fieldId, "FieldId cannot be null");
            return new HtmlString(FieldRendererService.RenderField(item, fieldId));
        }

        public static Item GetItem(string id)
        {
            return Context.Database.GetItem(new ID(id), Sitecore.Context.Language);
        }


        public static string GetFileUrl(Item item, ID assetId)
        {
            string mediaUrl = string.Empty;

            if (item.FieldHasValue(assetId))
            {
                FileField file = GetFile(item, assetId);
                if (file.MediaItem != null)
                {
                    mediaUrl = file.MediaItem.Url();
                }
                else
                {
                    Log.Warn($"ItemExtensions - GetFileUrl method has empty Media Item ", "Assignment.Foundation.SitecoreExtensions.Extensions.ItemExtensions");
                }              
            }
            return mediaUrl;
        }

        public static string GetFileExtension(Item item, ID assetId)
        {
            string extension = string.Empty;

            if (item.FieldHasValue(assetId))
            {
                MediaItem file = GetFile(item, assetId).MediaItem;
                if (file != null)
                {
                    extension = file.Extension;
                }
                else
                {
                    Log.Warn($"ItemExtensions - GetFileExtension method has empty File Item ", "Assignment.Foundation.SitecoreExtensions.Extensions.ItemExtensions");
                }
            }

            return extension;
        }

        public static double GetFileSize(Item item, ID assetId)
        {
            double size = 0;

            if (item.FieldHasValue(assetId))
            {
                MediaItem file = GetFile(item, assetId).MediaItem;
                if (file != null)
                {
                    size = (file.Size / 1024f) / 1024f;
                }
                else
                {
                    Log.Warn($"ItemExtensions - GetFileSize method has empty File Item ", "Assignment.Foundation.SitecoreExtensions.Extensions.ItemExtensions");
                }
            }

            return size;
        }



        public static FileField GetFile(Item item, ID assetId)
        {
            if (item.FieldHasValue(assetId))
                return (FileField)item.Fields[assetId];

            return null;
        }

        public static string GetSiteRootFieldValue(ID fieldID)
        {
            Item rootItem = SiteExtensions.GetRootItem(Sitecore.Context.Site);

            if(rootItem.FieldHasValue(fieldID))
            {
                return rootItem.Fields[fieldID].Value;
            }
         
            return string.Empty;
        }
    }

    public enum LinkFieldOption
    {
        Text,
        LinkType,
        Class,
        Alt,
        Target,
        QueryString
    }
}