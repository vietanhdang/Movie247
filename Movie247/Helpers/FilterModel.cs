using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Movie247.Helpers
{
    public class FilterModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Keyword { get; set; }
        public int[] CompanyId { get; set; }
        public string[] CountryId { get; set; }
        public int[] GenreId { get; set; }
        public string SortBy { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public string OrderBy { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public FilterModel()
        {
            SortBy = "popularity";
            OrderBy = "desc";
            Page = 1;
            PageSize = 12;
            TotalPages = 0;
        }
    }
}
