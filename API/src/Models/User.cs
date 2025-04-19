using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public required string Name { get; set; } = string.Empty;
        
        [Required]
        public required string LastName { get; set; } = string.Empty;
        
        
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        public required DateOnly BirthDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required]
        [EmailAddress]
        public required string Email { get; set; } = string.Empty;
        
        [Required]
        public required string Password { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        public DateOnly lastLogin { get; set; } = DateOnly.FromDateTime(DateTime.Now);


        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; } = new Role();

        public List<Address> Addresses { get; set; } = new List<Address>();

        public int? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address? Address { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

    }
}