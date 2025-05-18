using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.Interface
{
    public interface ICategoryRepository
    {
        public Task<bool> isEmpty();
        public Task<bool> SeedCategory(string name);
    }
}