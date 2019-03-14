namespace Assignment.Foundation.SitecoreExtensions.Extensions
{
    using System;
    using Sitecore;
    using Sitecore.Data.Fields;
    using Sitecore.Resources.Media;
    using Sitecore.Globalization;

    public static class FieldExtensions
    {
        public static string ImageUrl(this ImageField imageField)
        {
            if (imageField?.MediaItem == null)
            {
                throw new ArgumentNullException(nameof(imageField));
            }

            var options = MediaUrlOptions.Empty;
            int width, height;

            if (int.TryParse(imageField.Width, out width))
            {
                options.Width = width;
            }

            if (int.TryParse(imageField.Height, out height))
            {
                options.Height = height;
            }
            return imageField.ImageUrl(options);
        }

        public static string ImageUrl(this ImageField imageField, MediaUrlOptions options)
        {
            if (imageField?.MediaItem == null)
            {
                throw new ArgumentNullException(nameof(imageField));
            }

            return options == null ? imageField.ImageUrl() : HashingUtils.ProtectAssetUrl(MediaManager.GetMediaUrl(imageField.MediaItem, options));
        }

        public static bool IsChecked(this Field checkboxField)
        {
            if (checkboxField == null)
            {
                throw new ArgumentNullException(nameof(checkboxField));
            }
            return MainUtil.GetBool(checkboxField.Value, false);
        }

        public static string GetDateByLocal(this DateTime dateValue, string dateFormat)
        {
            string localSpecificDateValue = string.Empty;

            if (Sitecore.Context.Language.ToString().Equals("ar"))
            {
                Language arabicUAELanguage = Language.Parse("ar-ae");
                localSpecificDateValue = dateValue.ToString(dateFormat, arabicUAELanguage.CultureInfo);
            }
            else
            {
                localSpecificDateValue = dateValue.ToString(dateFormat, Sitecore.Context.Language.CultureInfo);
            }
            return localSpecificDateValue;
        }
    }
}