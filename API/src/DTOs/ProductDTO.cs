using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Models;

namespace API.src.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Price { get; set; }
        public string? ImageUrl { get; set; }

    }
}