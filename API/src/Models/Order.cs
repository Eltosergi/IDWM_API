using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public required string OrderNumber { get; set; }
        [Required]
        public required Cart Cart { get; set; }

        [Required]
        public required int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address? Address { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

    }
}