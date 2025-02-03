using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository;

public class UserRepository : IUserRepository<User>
{
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;

    public UserRepository(UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task<User> ValidatePass(User entity)
    {
        var userFind = await _userManager.FindByEmailAsync(entity.Email);
        if (userFind == null)
        {
            return null;
        }
        var passFind = await _signInManager.PasswordSignInAsync(userFind, entity.PasswordHash, false, false);
        if (!passFind.Succeeded)
        {
            return null;
        }

        return userFind;
    }

    public async Task<IdentityResult> Register(User entity)
    {
        var result = await _userManager.CreateAsync(entity, entity.PasswordHash);
        
        return result;
    }

    public async Task SetUserRole(User entity, string role)
    {
        await _userManager.AddToRoleAsync(entity, role);
    }

    public async Task<User> GetUserInfo(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        return user;
    }

    public async Task<IdentityResult> UpdateInfo(User entity)
    {
        var user = await _userManager.UpdateAsync(entity);
        return user;
    }

    public async Task<IdentityResult> DeleteUser(User entity)
    {
        var result = await _userManager.DeleteAsync(entity);
        return result;
    }


    public async Task<IList<string>> GetRoles(User entity)
    {
        return await _userManager.GetRolesAsync(entity);
    }
}