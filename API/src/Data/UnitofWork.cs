using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Interface;

namespace API.src.Data
{
    public class UnitofWork :IUnitofWork
    {
        public IUserRepository _userRepository { get; }
        public IAddressRepository _addressRepository { get; }
        public IBrandRepository _brandRepository { get; }
        public IConditionRepository _conditionRepository { get; }
        public IProductRepository _productRepository { get; }
        private readonly AplicationDbContext _context;

        public UnitofWork(AplicationDbContext context,IUserRepository userRepository,IAddressRepository addressRepository,IBrandRepository brandRepository,IConditionRepository conditionRepository,IProductRepository productRepository)
        {
            _context=context;
            _userRepository=userRepository;
            _addressRepository=addressRepository;
            _brandRepository=brandRepository;
            _conditionRepository=conditionRepository;
            _productRepository=productRepository;
        }

        public async Task<int> Save()
        => await _context.SaveChangesAsync();
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}