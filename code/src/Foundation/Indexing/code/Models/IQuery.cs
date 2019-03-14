namespace Assignment.Foundation.Indexing.Models
{
    using System;
    using System.Collections.Generic;

    public interface IQuery
    {
        string QueryText { get; set; }
        int NoOfResults { get; set; }
        Dictionary<string, string[]> Facets { get; set; }
        int Page { get; set; }
        Dictionary<string, bool> Sort { get; set; }
        DateTime? PublicationDateStart { get; set; }
        DateTime? PublicationDateEnd { get; set; }
        string SearchPath { set; get; }
    }
}