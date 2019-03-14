namespace Assignment.Foundation.Indexing.Services
{
    using Sitecore.ContentSearch;
    using DependencyInjection;

    [Service]
    public class SearchIndexResolver
    {
        public virtual ISearchIndex GetIndex(SitecoreIndexableItem contextItem)
        {
            string index = "assignment_web_index";
            var dbName = Sitecore.Context.Database.Name;
            if (dbName.Equals("master", System.StringComparison.InvariantCultureIgnoreCase))
            {
                index = "assignment_master_index";
            }
            return ContentSearchManager.GetIndex(index);
        }
    }
}