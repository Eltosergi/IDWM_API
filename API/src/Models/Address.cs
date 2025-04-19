using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.src.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        public string? Department { get; set; } = string.Empty;


    }
}
