using Ecommerce.DTOs;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository;

public class ProductRepository : IProductRepository<Product>
{
    private StoreContext _context;

    public ProductRepository(StoreContext context)
    {
        _context = context;
    }
    public async Task<Product> GetProductById(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task AddProduct(Product entity)
    { 
        await _context.AddAsync(entity);
    }

    public void UpdateProduct(Product entity)
    {
        _context.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void DeleteProduct(Product entity)
    {
        _context.Products.Remove(entity);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync(); 
    }
    
    public IEnumerable<Product> Search(Func<Product, bool> filter)
    {
        return _context.Products.Where(filter).ToList();
    }
}