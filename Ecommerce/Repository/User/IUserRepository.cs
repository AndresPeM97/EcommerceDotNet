using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Repository;

public interface IUserRepository<TEntity>
{
    Task<TEntity> ValidatePass(TEntity entity);
    Task<IdentityResult> Register(TEntity entity);
    Task SetUserRole(TEntity entity, string role);
    Task<TEntity> GetUserInfo(string email);
    Task<IdentityResult> UpdateInfo(TEntity entity);
    Task<IdentityResult> DeleteUser(TEntity entity);
    Task<IList<string>> GetRoles(TEntity entity);
}