using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models;

public class StoreContext : IdentityDbContext<User>
{
    public StoreContext(DbContextOptions<StoreContext> options)
        : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
}