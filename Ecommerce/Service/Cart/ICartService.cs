using Ecommerce.DTOs;
using Ecommerce.Models;

namespace Ecommerce.Service;

public interface ICartService
{
    Task<CartItemDto> GetItem(int productId, string userId);
    Task<CartItemDto> AddItem(int itemId, string user);
    Task<CartItemDto> DecreaseItem(int itemId, string user);
    Task<CartItemDto> RemoveItem(int itemId, string user);
    Task<List<CartItemDto>> GetItemsCart(string userId);
}