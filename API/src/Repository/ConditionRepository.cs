using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Data;
using API.src.Interface;
using API.src.Models;

using Microsoft.EntityFrameworkCore;

namespace API.src.Repository
{
    public class ConditionRepository : IConditionRepository
    {
        private readonly AplicationDbContext _context;

        public ConditionRepository(AplicationDbContext context)
        {
            _context = context;
        }
        public Task<bool> AddCondition(string condition)
        {
            var newCondition = new Condition { Name = condition };
            _context.Conditions.Add(newCondition);
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
        }

        public async Task<bool> isEmpty()
        {
            return !await _context.Conditions.AnyAsync();
        }
    }
}