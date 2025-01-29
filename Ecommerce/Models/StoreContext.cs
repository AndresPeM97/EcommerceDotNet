using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
}