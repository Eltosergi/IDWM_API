using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class CreateAddressDTO
    {
        [Required]
        public required string Street { get; set; } = string.Empty;

        [Required]
        public required int Number { get; set; }

        [Required]
        public required string Commune { get; set; } = string.Empty;

        [Required]
        public required string Region { get; set; } = string.Empty;

        [Required]
        public required int PostalCode { get; set; }
    }
}