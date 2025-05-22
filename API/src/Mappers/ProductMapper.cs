using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.DTOs;
using API.src.Models;

namespace API.src.Mappers
{
    public class ProductMapper
    {
        public static InsertionProdutCartDTO UserProductToInsertionProductCartDTO(User user, Product product) =>
        new InsertionProdutCartDTO
        {
            UserName = user.Name,
            ProductName = product.Name,
            Amount = 1
        };

        public static SuccessProduct ProductToSuccessProduct(Product product) =>
        new SuccessProduct
        {
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Stock = product.Stock,
            Condition = product.Condition?.Name ?? string.Empty,
            Brand = product.Brand?.Name ?? string.Empty,
            Category = product.Categories.Select(c => c.Name).ToList(),
            Images = product.Images.Select(i => i.Url).ToList()
        };



    }
}