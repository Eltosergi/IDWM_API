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
        public int Id { get; set; } = 0;
        [Required]
        public required string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public required int AddressId { get; set; }
        [ForeignKey("AddressId")] 
        public Address? Address { get; set; }

        public required int HistoricCartId { get; set; }
        [ForeignKey("HistoricCartId")]
        public HistoricCart? HistoricCart { get; set; }
         
    }
}