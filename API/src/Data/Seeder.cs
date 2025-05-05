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
        private readonly IUserRepository _userRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IConditionRepository _conditionRepository;

        private readonly IProductRepository _productRepository;
        private readonly Faker _faker;

        public Seeder(IUserRepository userRepository, IBrandRepository brandRepository, IConditionRepository conditionRepository, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _brandRepository = brandRepository;
            _conditionRepository = conditionRepository;

            _faker = new Faker("es_MX");
        }

        public async Task Seed()
        {
            await SeedBrands();
            await SeedConditions();
            await SeedUsers();

        }

        public async Task SeedUsers()
        {
            if (await _userRepository.isEmpty())
            {
                var AdminUser = new CreateUserDTO
                {
                    Name = "Admin",
                    LastName = "Admin",
                    BirthDate = DateOnly.FromDateTime(DateTime.Now),
                    Email = "Admin@Admin.com",
                    Password = "@Admin1234"
                };

                await _userRepository.CreateAdmin(AdminUser);
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

    }
}