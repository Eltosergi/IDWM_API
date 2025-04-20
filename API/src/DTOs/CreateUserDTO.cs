using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        public required string Name { get; set; } 
        [Required]
        public required string LastName { get; set; } 
        [Required]
        public required DateOnly BirthDate { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public int RoleId { get; set; } = 1; 

    }
}