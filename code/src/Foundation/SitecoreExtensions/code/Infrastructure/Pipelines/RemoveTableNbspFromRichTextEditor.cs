namespace Assignment.Foundation.SitecoreExtensions.Infrastructure.Pipelines
{
    using Sitecore.Shell.Controls.RichTextEditor.Pipelines.SaveRichTextContent;
    using HtmlAgilityPack;
    public class RemoveTableNbspFromRichTextEditor
    {
        public void Process(SaveRichTextContentArgs args)
        {
            if (args.Content == null || string.IsNullOrEmpty(args.Content)) return;

            args.Content = FormaTable(args.Content);            
        }

        private string FormaTable(string content)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            if (!doc.DocumentNode.InnerHtml.Contains("table"))
                return content;

            foreach (var td in doc.DocumentNode.Descendants("td"))
            {
                HtmlNode tdNode = td;
                if (tdNode.InnerText.Contains("&nbsp;"))
                {
                    tdNode.InnerHtml = tdNode.InnerText.Replace("&nbsp;", "");
                }
            }

            return doc.DocumentNode.OuterHtml;
        }       

    }
}