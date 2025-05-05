using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class AuthenticatedUserDto
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public string Token { get; set; } = null!;

        public DateTime RegisteredAt { get; set; }
        public DateTime? LastAccess { get; set; }
        public bool IsActive { get; set; }
    }
}