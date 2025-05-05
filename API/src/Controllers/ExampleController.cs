using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.src.Controllers
{
    public class ExampleController:BaseController
    {
        [Authorize]
        [HttpGet("example")]
        public IActionResult GetExample()
        {
            return Ok("Wena campeon!, pudiste acceder a la ruta protegida por JWT");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("exampleAdmin")]
        public IActionResult GetExampleAdmin()
        {
            return Ok("Buenos días Admin, pudiste acceder a la ruta protegida por JWT y el rol Admin");
        }

        [HttpGet("exampleUser")]
        public IActionResult GetExampleUser()
        {
            
            if (User?.Identity?.IsAuthenticated != true)
            {
                return StatusCode(401, new { message = "Logeate primero si no eres nadie" });
            }
            if (!User.IsInRole("Admin"))
            {
                return StatusCode(403, new { message = "Usted no debería estar aquí, lárguese, no le quiero ver" });
            }
            return Ok("Buenos días Admin, pudiste acceder a la ruta protegida por JWT y el rol Admin");

        }

    }
}