using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class QueryParamsProduct
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        [RegularExpression("^(Nuevo|Usado)$", ErrorMessage = "La condición debe ser 'Nuevo' o 'Usado'.")]
        public string? Condition { get; set; }
        public IEnumerable<string>? Brands { get; set; }
        public IEnumerable<string>? Categories { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }

        public string? OrderBy { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;



    }
}