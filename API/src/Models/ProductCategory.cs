using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.Models
{
    public class ProductCategory
    {
        [Required]
        public required int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required]
        public required int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}