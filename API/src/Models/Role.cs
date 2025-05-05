using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace API.src.Models
{
    public class Role:IdentityRole<int>
    {
    }
}