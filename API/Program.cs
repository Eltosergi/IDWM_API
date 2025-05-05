using System.Security.Claims;
using System.Text;

using API.src.Data;
using API.src.Interface;
using API.src.Models;
using API.src.Repository;
using API.src.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Servicios
builder.Services.AddControllers();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IConditionRepository, ConditionRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<AplicationDbContext>();
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SignInKey"]!)),
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddDbContext<AplicationDbContext>(options =>options.UseSqlite(connectionString));

builder.Services.AddTransient<Seeder>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<AplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var seeder = services.GetRequiredService<Seeder>();
   await seeder.Seed();
}

app.Run();