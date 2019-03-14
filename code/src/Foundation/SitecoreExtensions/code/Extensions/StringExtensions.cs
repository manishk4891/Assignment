namespace Assignment.Foundation.SitecoreExtensions.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string Humanize(this string input)
        {
            return Regex.Replace(input, "(\\B[A-Z])", " $1");
        }

        public static string ToCssUrlValue(this string url)
        {
            return string.IsNullOrWhiteSpace(url) ? "none" : $"url('{url}')";
        }
        public static string SentenceCase(this string title)
        {
            List<string> stringList = title.Split(' ').ToList();
            StringBuilder titleValue = new StringBuilder();
            foreach (var item in stringList)
            {
                titleValue.Append(item.Substring(0, 1).ToUpper());
                titleValue.Append(item.Substring(1).ToLower());
                titleValue.Append(" ");
            }
            return titleValue.ToString();
        }

    }
}