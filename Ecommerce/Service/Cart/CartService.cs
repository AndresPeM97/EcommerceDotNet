using AutoMapper;
using Ecommerce.DTOs;
using Ecommerce.Models;
using Ecommerce.Repository;
using Microsoft.AspNetCore.Razor.Language;

namespace Ecommerce.Service;

public class CartService : ICartService
{
    private ICartRepository<Cart> _cartRepository;
    private IMapper _mapper;
    private IUserRepository<User> _userRepository;

    public CartService(ICartRepository<Cart> cartRepository,
        IMapper mapper, IUserRepository<User> userRepository)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<CartItemDto> GetItem(int productId, string user)
    {
        var userFind = await _userRepository.GetUserInfo(user);
        if(userFind == null){return null;}
        
        var item = await _cartRepository.GetItem(productId, userFind.Id);
        
        return _mapper.Map<CartItemDto>(item);
    }

    public async Task<CartItemDto> AddItem(int itemId, string user)
    {
        var itemDto = new CartItemDto();
        var userFind = await _userRepository.GetUserInfo(user);
        if(userFind == null){return null;}
        
        var item = await _cartRepository.GetItem(itemId, userFind.Id);
        if (item == null)
        {
            var newItem = new Cart
            {
                UserId = userFind.Id,
                ProductId = itemId,
                Quantity = 1
            };

            await _cartRepository.AddItem(newItem);
            await _cartRepository.SaveChanges();
            
            item = await _cartRepository.GetItem(itemId, userFind.Id);
            
            itemDto = _mapper.Map<CartItemDto>(item);
            itemDto.Total = itemDto.Price;
            return itemDto;
        }
        
        item.Quantity += 1;
        _cartRepository.UpdateItem(item);
        await _cartRepository.SaveChanges();
        
        itemDto = _mapper.Map<CartItemDto>(item);
        itemDto.Total = item.Quantity * itemDto.Price;
        return itemDto;
    }

    public async Task<CartItemDto> DecreaseItem(int itemId, string user)
    {
        var itemDto = new CartItemDto();
        var userFind = await _userRepository.GetUserInfo(user);
        if(userFind == null){return null;}
        
        var item = await _cartRepository.GetItem(itemId, userFind.Id);
        if (item == null)
        {
            return null;
        }
        
        item.Quantity -= 1;
        if (item.Quantity == 0)
        {
            _cartRepository.RemoveItem(item);
        }
        else
        {
            _cartRepository.UpdateItem(item);
        }
        await _cartRepository.SaveChanges();
        
        itemDto = _mapper.Map<CartItemDto>(item);
        itemDto.Total = item.Quantity * itemDto.Price;
        return itemDto;

    }

    public async Task<CartItemDto> RemoveItem(int itemId, string user)
    {
        var userFind = await _userRepository.GetUserInfo(user);
        if(userFind == null){return null;}
        
        var item = await _cartRepository.GetItem(itemId, userFind.Id);
        if (item == null)
        {
            return null;
        }
        
        _cartRepository.RemoveItem(item);
        await _cartRepository.SaveChanges();
        
        return _mapper.Map<CartItemDto>(item);
    }

    public async Task<List<CartItemDto>> GetItemsCart(string userEmail)
    {
        var userId = await _userRepository.GetUserInfo(userEmail);
        var items = await _cartRepository.GetItems(userId.Id);
        var itemsDto = items.Select(x => _mapper.Map<CartItemDto>(x)).ToList();

        foreach (var cartItemDto in itemsDto)
        {
            cartItemDto.Total = cartItemDto.Price * cartItemDto.Quantity;
        }
        
        return itemsDto;
    }
}