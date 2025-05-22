using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using API.src.Data;
using API.src.DTOs;
using API.src.Extensions;
using API.src.Helpers;
using API.src.Interface;
using API.src.Mappers;
using API.src.Models;
using API.src.RequestHelpers;
using API.Src.Extensions;

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var product = await _unitofWork.ProductRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound(new ApiResponse<string>(false, "Product not found"));
            }

            var mappedProduct = ProductMapper.ProductToSuccessProduct(product);

            return Ok(new ApiResponse<SuccessProduct>(true, "Product obtained successfully", mappedProduct));
        }

        public async Task<IActionResult> GetProductsAsync([FromQuery] QueryParamsProduct queryParams)
        {
            var query = _unitofWork.ProductRepository.GetQueryableProducts();

            query = query
                .Filter(queryParams.Id, queryParams.Brands, queryParams.Categories, queryParams.MinPrice, queryParams.MaxPrice, queryParams.Condition)
                .Search(queryParams.Name)
                .Sort(queryParams.OrderBy);

            var pagedList = await PagedList<Product>.ToPagedList(query, queryParams.PageNumber, queryParams.PageSize);

            if (pagedList == null || pagedList.Count == 0)
                return Ok(new ApiResponse<IEnumerable<Product>>(false, "No hay productos disponibles"));


            Response.AddPaginationHeader(pagedList.Metadata);

            var mappedProducts = pagedList.Select(ProductMapper.ProductToSuccessProduct);

            return Ok(new ApiResponse<IEnumerable<SuccessProduct>>(
                true,
                "Productos obtenidos correctamente",
                mappedProducts
            ));
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

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] CreateProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, "Validation failed"));
            }
            try
            {
                var result = await _unitofWork.ProductRepository.UpdateProduct(id, product);
                return Ok(new ApiResponse<SuccessProduct>(true, "Product updated successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                return StatusCode(500, new ApiResponse<string>(false, "Internal server error"));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, "Validation failed"));
            }
            try
            {
                var result = await _unitofWork.ProductRepository.DeleteProduct(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<string>(false, "Product not found"));
                }

                await _unitofWork.SaveChangeAsync();
                return Ok(new ApiResponse<SuccessProduct>(true, "Product deleted successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return StatusCode(500, new ApiResponse<string>(false, "Internal server error"));
            }
        }





    }
}