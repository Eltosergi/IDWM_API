using API.src.Data;
using API.src.Interface;
using API.src.Repository;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AplicationDbContext>(options =>
    options.UseSqlite(connectionString));


builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IConditionRepository, ConditionRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddTransient<Seeder>();

var app = builder.Build();


await using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<AplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var seeder = services.GetRequiredService<Seeder>();
    await seeder.Seed();
}

app.Run();