namespace Assignment.Foundation.Indexing.Repositories
{
    using Assignment.Foundation.Indexing.Models;
    using Assignment.Foundation.Indexing.Services;

    public interface ISearchServiceRepository
    {
        SearchService Get(ISearchSettings searchSettings);
    }
}