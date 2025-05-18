using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.src.Models
{
    public class CartProduct
    {

        [Required]
        public required int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required]
        public required int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart? Cart { get; set; }
        public int Quantity { get; set; } = 1;


        public int TotalPrice => (Product?.Price ?? 0) * Quantity;
    }
}