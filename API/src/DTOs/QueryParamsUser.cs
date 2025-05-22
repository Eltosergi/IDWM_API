using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class QueryParamsUser
    {
        public int? Id { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? RegisteredFrom { get; set; }
        public DateTime? RegisteredTo { get; set; }
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "dateDesc";

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}