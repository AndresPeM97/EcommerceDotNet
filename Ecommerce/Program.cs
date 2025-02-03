using System.Text;
using Ecommerce.AutoMappers;
using Ecommerce.DTOs;
using Ecommerce.Models;
using Ecommerce.Repository;
using Ecommerce.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Services
builder.Services.AddScoped<IUserLoginService<UserLoginDto>, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
//Repository
builder.Services.AddScoped<IUserRepository<User>, UserRepository>();
builder.Services.AddScoped<IProductRepository<Product>, ProductRepository>();

// DBContext
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("StoreConnection"), new MySqlServerVersion(new Version(9, 2, 0)));
});
//Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<StoreContext>()
    .AddDefaultTokenProviders();

async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
    string[] roleNames = { "Admin", "User", "Customer" };

    foreach (var role in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(role);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
        
}


//Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

// JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero,
        ValidateLifetime = true
    };
})
.AddCookie(options => 
{
    options.LoginPath = "login";
    // options.AccessDeniedPath = "/Account/AccessDenied";
});
builder.Services.AddAuthorization();
// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()  // Permite cualquier origen
            .AllowAnyMethod()  // Permite cualquier método
            .AllowAnyHeader(); // Permite cualquier encabezado
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRoles(services);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();