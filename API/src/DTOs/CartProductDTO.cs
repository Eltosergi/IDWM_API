using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class CartProductDTO
    {
        public required string Name { get; set; } 
        public required int Amount { get; set; }
    }
}