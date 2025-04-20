using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.Interface
{
    public interface IConditionRepository
    {
        Task<bool> isEmpty();
        Task<bool> AddCondition(string condition);
    }
}