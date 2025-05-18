using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class CartProductDTO
    {
        public required ProductDTO Product { get; set; } = new();
        public required int Quantity { get; set; }
    }
}