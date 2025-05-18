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


    }
}