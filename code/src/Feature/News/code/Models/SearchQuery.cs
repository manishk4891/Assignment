using System;
using System.Collections.Generic;
using Assignment.Foundation.Indexing.Models;

namespace Assignment.Feature.News.Models
{
    public class SearchQuery : IQuery
    {
        public string QueryText { get; set; }
        public int Page { get; set; }
        public int NoOfResults { get; set; }
        public Dictionary<string, string[]> Facets { get; set; }
        public Dictionary<string,bool> Sort { get; set; }
        public DateTime? PublicationDateStart { get; set; }
        public DateTime? PublicationDateEnd { get; set; }

        public string SearchPath { set; get; }

    }
}