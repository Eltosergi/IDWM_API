using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.src.Models
{
    public class HistoricCartProduct
    {
        [Required]
        public required int HistoricCartId { get; set; }
        [ForeignKey("HistoricCartId")]
        public HistoricCart? Cart { get; set; }

        [Required]
        public required int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
