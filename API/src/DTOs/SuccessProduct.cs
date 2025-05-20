using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class SuccessProduct
    {
        public required string Name { get; set; }
        public required int Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; } = 0;
        public required string Condition { get; set; }
        public required string Brand { get; set; }

    }
}