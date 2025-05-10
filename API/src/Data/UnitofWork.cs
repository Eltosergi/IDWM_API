using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.Interface;

namespace API.src.Data
{
    public class UnitofWork 
    {
        public IUserRepository UserRepository { get; }
        public IAddressRepository AddressRepository { get; }
        public IBrandRepository BrandRepository { get; }
        public IConditionRepository ConditionRepository { get; }
        public IProductRepository ProductRepository { get; }
        private readonly AplicationDbContext _context;

        public UnitofWork(AplicationDbContext context,IUserRepository userRepository,IAddressRepository addressRepository,IBrandRepository brandRepository,IConditionRepository conditionRepository,IProductRepository productRepository)
        {
            _context=context;
            UserRepository=userRepository;
            AddressRepository=addressRepository;
            BrandRepository=brandRepository;
            ConditionRepository=conditionRepository;
            ProductRepository=productRepository;
        }

        public async Task<int> SaveChangeAsync() =>  await _context.SaveChangesAsync();
        
 
        
    }
}