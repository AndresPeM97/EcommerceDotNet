using Ecommerce.Models;

namespace Ecommerce.Repository;

public interface IProductRepository<TEntity>
{
    Task<TEntity> GetProductById(int id);
    Task<IEnumerable<TEntity>> GetProducts();
    Task AddProduct(TEntity entity);
    void UpdateProduct(TEntity entity);
    void DeleteProduct(TEntity entity);
    Task SaveChanges();

    IEnumerable<Product> Search(Func<Product, bool> filter);

}