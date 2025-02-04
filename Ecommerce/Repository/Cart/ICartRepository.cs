namespace Ecommerce.Repository;

public interface ICartRepository<TEntity>
{
    Task<TEntity> GetItem(int id, string userId);
    Task AddItem(TEntity item);
    void UpdateItem(TEntity item);
    void RemoveItem(TEntity item);
    Task SaveChanges();
    Task<IEnumerable<TEntity>> GetItems(string userId);
}