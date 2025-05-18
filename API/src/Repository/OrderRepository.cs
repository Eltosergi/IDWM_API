using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using API.src.Data;
using API.src.Interface;
using API.src.Models;

using Microsoft.EntityFrameworkCore;

namespace API.src.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AplicationDbContext _context;

        public OrderRepository(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrder(int selection, int userId)
        {
            if (selection < 0)
                throw new ArgumentOutOfRangeException(nameof(selection), "Selection must be non-negative.");

            var user = await _context.Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.Products)
                        .ThenInclude(cp => cp.Product)
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (user.Addresses == null || selection >= user.Addresses.Count)
                throw new ArgumentOutOfRangeException(nameof(selection), "Invalid address selection.");

            var order = new Order
            {
                OrderNumber = GenerarCodigo(),
                Cart = user.Cart ?? throw new InvalidOperationException("User cart is null."),
                AddressId = user.Addresses.ElementAt(selection).Id
            };

            _context.Orders.Add(order);
            user.Orders.Add(order);
            user.Cart = new Cart { Products = new List<CartProduct>() };

            await _context.SaveChangesAsync();

            return true;
        }






        public static string GenerarCodigo(int largo = 20)
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var resultado = new StringBuilder(largo);
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                for (int i = 0; i < largo; i++)
                {
                    rng.GetBytes(bytes);
                    // Convierte bytes a un entero positivo
                    int valor = BitConverter.ToInt32(bytes, 0) & int.MaxValue;
                    resultado.Append(caracteres[valor % caracteres.Length]);
                }
            }
            return resultado.ToString();
        }
    }
}