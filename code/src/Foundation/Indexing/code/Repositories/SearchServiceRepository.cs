namespace Assignment.Foundation.Indexing.Repositories
{
    using Assignment.Foundation.DependencyInjection;
    using Assignment.Foundation.Indexing.Models;
    using Assignment.Foundation.Indexing.Services;

    [Service(typeof(ISearchServiceRepository))]
    public class SearchServiceRepository : ISearchServiceRepository
    {
        public virtual SearchService Get(ISearchSettings searchSettings)
        {
            return new SearchService(searchSettings);
        }
    }
}