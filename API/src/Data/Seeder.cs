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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly Faker _faker;

        public Seeder(IUserRepository userRepository, IBrandRepository brandRepository, IConditionRepository conditionRepository, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _brandRepository = brandRepository;
            _conditionRepository = conditionRepository;
            _categoryRepository = categoryRepository;

            _faker = new Faker("es_MX");
        }

        public async Task Seed()
        {
            await SeedBrands();
            await SeedConditions();
            await SeedCategories();
            await SeedUsers();
            await SeedProducts();

        }

        public async Task SeedUsers()
        {
            if (await _userRepository.isEmpty())
            {
                var AdminUser = new SeederUserDTO
                {
                    Name = "Admin",
                    LastName = "Admin",
                    BirthDate = DateOnly.FromDateTime(DateTime.Now),
                    Email = "Admin@Admin.com",
                    Password = "@Admin1234"
                };

                await _userRepository.SeederUser(AdminUser, "Admin");

            }

        }

        public async Task SeedBrands()
        {
            if (await _brandRepository.isEmpty())
            {
                for (int i = 0; i < 10; i++)
                {
                    var brandName = _faker.Commerce.ProductAdjective();

                    await _brandRepository.AddBrand(brandName);
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
                for (int i = 0; i < 10; i++)
                {
                    var product = new SeederProductDTO
                    {
                        Name = _faker.Commerce.ProductName(),
                        Price = _faker.Random.Int(100, 10000),
                        BrandId = _faker.Random.Int(1, 10),
                        ConditionId = _faker.Random.Int(1, 2)
                    };

                    var result = await _productRepository.SeedProduct(product);
                    if (!result)
                    {
                        throw new Exception("Error al crear el producto");
                    }



                }
            }
        }

        public async Task SeedCategories()
        {
            for (int i = 0; i < 10; i++)
            {
                var categoryName = _faker.Commerce.Categories(1).FirstOrDefault();
                if (categoryName != null)
                {
                    await _categoryRepository.SeedCategory(categoryName);
                }
            }
        }
    }
}