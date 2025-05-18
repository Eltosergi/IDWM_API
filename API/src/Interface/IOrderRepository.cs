using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Models;

namespace API.src.Interface
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(int selection, int userId);
    }
}