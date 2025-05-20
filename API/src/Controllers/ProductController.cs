using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using API.src.Data;
using API.src.DTOs;
using API.src.Helpers;
using API.src.Interface;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.src.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly UnitofWork _unitofWork;

        public ProductController(ILogger<ProductController> logger, UnitofWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, "Validation failed"));
            }

            try
            {
                var result = await _unitofWork.ProductRepository.CreateProduct(product);
                return Ok(new ApiResponse<SuccessProduct>(true, "Product created successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, new ApiResponse<string>(false, "Internal server error"));
            }
        }

    }
}