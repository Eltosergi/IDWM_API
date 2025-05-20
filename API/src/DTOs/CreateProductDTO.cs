using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class CreateProductDTO
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public required int Price { get; set; }

        public string Description { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number.")]
        public int Stock { get; set; } = 0;

        public bool IsActive { get; set; } = true;


        [Required]
        [RegularExpression("^(Nuevo|Usado)$", ErrorMessage = "La condición debe ser 'Nuevo' o 'Usado'.")]
        public required string Condition { get; set; }

        private string _brand = string.Empty;

        [Required]
        public required string Brand
        {
            get => _brand;
            set => _brand = value?.ToUpperInvariant() ?? string.Empty;
        }

        public ICollection<string> ImageUrl { get; set; } = new List<string>();
        private ICollection<string> _categories = new List<string>();

        public ICollection<string> Categories
        {
            get => _categories;
            set => _categories = value?.Select(c => c.ToUpperInvariant()).ToList() ?? new List<string>();
        }
    }
}