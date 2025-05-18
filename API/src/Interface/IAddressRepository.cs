using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.DTOs;

using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace API.src.Interface
{
    public interface IAddressRepository
    {
        public Task<bool> isEmpty();
        public Task<bool> CreateAddress(AddressDTO addressDTO, int userId);
        public Task<ICollection<AddressDTO>> GetAddress(int userId);
        public Task<bool> DeleteAddress(int addressId, int userId);

    }
}