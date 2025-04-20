using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.Interface
{
    public interface IRoleRepository
    {
        Task<bool> isEmpty();
        Task<bool> createRole(string roleName);
    }
}