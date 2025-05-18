using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class SeederProductDTO
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
        [Required]
        public required int Price { get; set; } = 0;
        [Required]
        public required int BrandId { get; set; } = 0;
        [Required]
        public required int ConditionId { get; set; } = 0;
    }
}