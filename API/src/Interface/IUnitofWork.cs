using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.Interface
{
    public interface IUnitofWork :IDisposable
    {
        IUserRepository _userRepository { get; }
        IAddressRepository _addressRepository { get; }
        IBrandRepository _brandRepository { get; }
        IConditionRepository _conditionRepository { get; }
        IProductRepository _productRepository { get; }
        Task<int> Save();
    }
    
}