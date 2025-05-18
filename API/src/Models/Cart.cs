using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.src.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ICollection<CartProduct> Products { get; set; } = new List<CartProduct>();

        public int Total => Products.Sum(p => p.TotalPrice);
    }
}