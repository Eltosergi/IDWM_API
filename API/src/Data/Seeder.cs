using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.src.DTOs;
using API.src.Interface;

using Bogus;

namespace API.src.Data
{
    public class Seeder
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IConditionRepository _conditionRepository;

        private readonly IProductRepository _productRepository;
        private readonly Faker _faker;

        public Seeder(IRoleRepository roleRepository, IUserRepository userRepository, IBrandRepository brandRepository, IConditionRepository conditionRepository, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _brandRepository = brandRepository;
            _conditionRepository = conditionRepository;

            _faker = new Faker("es_MX");
        }
        
        public async Task Seed()
        {
            await SeedRoles();
            await SeedUsers();
            await SeedBrands();
            await SeedConditions();
            await SeedProducts();
            
        }
        public async Task SeedRoles()
        {
            if (await _roleRepository.isEmpty())
            {
                await _roleRepository.createRole("Admin");
                await _roleRepository.createRole("User");
            }
        }
        public async Task SeedUsers()
        {
            if (await _userRepository.isEmpty())
            {
                var users = new Faker<CreateUserDTO>()
                    .CustomInstantiator(f => new CreateUserDTO
                    {
                        Name = f.Name.FirstName(),
                        LastName = f.Name.LastName(),
                        BirthDate = DateOnly.FromDateTime(f.Date.Past(80, DateTime.Today.AddYears(-18))), 
                        Email = f.Internet.Email(),
                        Password = f.Internet.Password(8, false, "", "A1!"),  
                        RoleId = f.PickRandom(new[] { 1, 2 })
                    })
                    .Generate(10);

                foreach (var user in users)
                {
                    await _userRepository.createUser(user);
                }
            }
        }

        public async Task SeedBrands()
        {
            if (await _brandRepository.isEmpty())
            {
                var brandFaker = new Faker<string>()
                    .CustomInstantiator(f => f.Company.CompanyName());

                var fakeBrands = brandFaker.Generate(10); 

                foreach (var brand in fakeBrands.Distinct()) 
                {
                    await _brandRepository.AddBrand(brand);
                }
            }
        }

        public async Task SeedConditions()
        {
            if (await _conditionRepository.isEmpty())
            {
                await _conditionRepository.AddCondition("Nuevo");
                await _conditionRepository.AddCondition("Usado");
            }
        }

        public async Task SeedProducts()
        {
            if (await _productRepository.isEmpty())
            {
                var products = new Faker<CreateProductDTO>()
                    .CustomInstantiator(f => new CreateProductDTO
                    {
                        Name = f.Commerce.ProductName(),
                        Price = (int)f.Finance.Amount(100, 10000),
                        BrandId = f.PickRandom(new[] { 1, 2, 3, 4, 5 }),
                        ConditionId = f.PickRandom(new[] { 1, 2 })
                    })
                    .Generate(100);

                foreach (var product in products)
                {
                    await _productRepository.CreateProduct(product);
                }
            }
        }


        

    }
}