using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository;

public class UserRepository : IUserRepository<User>
{
    private StoreContext _context;

    public UserRepository(StoreContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<User>> Get()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        _context.Users.Attach(user);
        _context.Entry(user).State = EntityState.Modified;
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

    public IEnumerable<User> Search(Func<User, bool> filter)
    {
        return _context.Users.Where(filter);
    }
}