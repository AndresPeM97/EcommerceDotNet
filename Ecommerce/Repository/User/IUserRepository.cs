namespace Ecommerce.Repository;

public interface IUserRepository<TEntity>
{
    Task<IEnumerable<TEntity>> Get();
    Task Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task Save();
    public IEnumerable<TEntity> Search(Func<TEntity, bool> filter);
}