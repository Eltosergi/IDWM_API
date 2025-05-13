using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.DTOs;
using API.src.Models;

namespace API.src.Interface
{
    public interface IProductRepository
    {
        public Task<bool> isEmpty();
        public Task<bool> CreateProduct(CreateProductDTO product);

        public Task<Product> GetProductByIdAsync(int id);
    }
}