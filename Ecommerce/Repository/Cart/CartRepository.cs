using Ecommerce.DTOs;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository;

public class CartRepository : ICartRepository<Cart>
{
    private StoreContext _context;

    public CartRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetItem(int id, string userId)
    {   
        var item = await _context.Carts
            .Include(a => a.Product) // ← Asegura que carga la relación
            .FirstOrDefaultAsync(a => a.ProductId == id && a.UserId == userId);

        return item;
    }

    public async Task AddItem(Cart item)
    {
        await _context.Carts.AddAsync(item);
    }

    public void UpdateItem(Cart item)
    {
        _context.Carts.Attach(item);
        _context.Entry(item).State = EntityState.Modified;
    }

    public void RemoveItem(Cart item)
    {
        _context.Carts.Remove(item);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Cart>> GetItems(string userId)
    {
        var items = await _context.Carts
            .Include(a => a.Product) // ← Asegura que carga la relación
            .Where(a => a.UserId == userId)
            .ToListAsync();
        return items;
    }
    
    
}