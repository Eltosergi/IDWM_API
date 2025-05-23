using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace API.src.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string LastName { get; set; }
        [Required]
        public required DateOnly BirthDate { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime lastLogin { get; set; } = DateTime.UtcNow;


        public bool IsActive { get; set; } = true;
        public string? ResonDeactivation { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        //User Address
        public ICollection<Address> Addresses { get; set; } = new List<Address>();


        //User Cart
        [Required]
        public required Cart Cart { get; set; }

    }
}