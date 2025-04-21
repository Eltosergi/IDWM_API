using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Data;
using API.src.DTOs;
using API.src.Interface;
using API.src.Models;

using Microsoft.EntityFrameworkCore;

namespace API.src.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AplicationDbContext _context;

        public AddressRepository(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> isEmpty()
        {
            return await _context.Addresses.AnyAsync();
        }

        public async Task<bool> CreateAddress(CreateAddressDTO addressDTO)
        {
            try
            {
                var address = new Address
                {
                    Street = addressDTO.Street,
                    Number = addressDTO.Number,
                    Commune = addressDTO.Commune,
                    Region = addressDTO.Region,
                    PostalCode = addressDTO.PostalCode
                };

                await _context.Addresses.AddAsync(address);
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    Console.WriteLine("No se guardó la dirección en la base de datos.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar dirección: {ex.Message}");
                return false;
            }
        }

    }
}