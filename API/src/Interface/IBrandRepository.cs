using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.Interface
{
    public interface IBrandRepository
    {
        Task<bool> isEmpty();
        Task<bool> AddBrand(string brand);
    }
}