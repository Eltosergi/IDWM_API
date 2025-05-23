using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }
        [Required]
        public required int Price { get; set; }

        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public int? ConditionId { get; set; }
        [ForeignKey("ConditionId")]
        public Condition? Condition { get; set; }



        public int? BrandId { get; set; }
        [ForeignKey("BrandId")]
        public Brand? Brand { get; set; }


        //Imagenes del producto
        public ICollection<Image> Images { get; set; } = new List<Image>();

        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}