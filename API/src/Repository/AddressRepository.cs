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

        public async Task<bool> CreateAddress(AddressDTO addressDTO, int userId)
        {
            var user = await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return false;

            var address = new Address
            {
                Street = addressDTO.Street,
                Number = addressDTO.Number,
                Commune = addressDTO.Commune,
                Region = addressDTO.Region,
                PostalCode = addressDTO.PostalCode,
                Department = addressDTO.Department ?? string.Empty
            };

            user.Addresses.Add(address);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<AddressDTO>> GetAddress(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new List<AddressDTO>();

            var addressDTOs = user.Addresses.Select(address => new AddressDTO
            {
                Street = address.Street,
                Number = address.Number,
                Commune = address.Commune,
                Region = address.Region,
                PostalCode = address.PostalCode,
                Department = address.Department
            }).ToList();

            return addressDTOs;
        }
        public async Task<bool> DeleteAddress(int addressId, int userId)
        {
            var user = await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return false;

            var address = user.Addresses.FirstOrDefault(a => a.Id == addressId);

            if (address == null)
                return false;

            _context.Addresses.Remove(address);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Puedes loguear esto o lanzarlo de nuevo si prefieres
                Console.WriteLine($"Error al eliminar la dirección: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
        }


    }
}