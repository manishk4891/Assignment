namespace Assignment.Foundation.Indexing.Models
{
    using System.Collections.Generic;

    public interface IQuerySort
    {
        string Title { get; set; }
        string FieldName { get; set; }
        bool Descending { get; set; }
    }
}