using System.Security.Claims;
using Ecommerce.DTOs;
using Ecommerce.Models;
using Ecommerce.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet("mycart")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetAllItems()
        {
            var owner = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            var items = await _cartService.GetItemsCart(owner);
            
            return items.Any() ? Ok(items) : NoContent();
            
        }

        [Authorize]
        [HttpGet("mycart/{id}")]
        public async Task<ActionResult<CartItemDto>> GetItem(int id)
        {
            var owner = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var item = await _cartService.GetItem(id, owner);
            
            return item != null ? Ok(item) : NotFound();
        }

        [Authorize]
        [HttpPost("mycart/add/{id}")]
        public async Task<ActionResult<CartItemDto>> AddItemToCart(int id)
        {
            var owner = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            var item = await _cartService.AddItem(id, owner);
            
            return item != null ? Ok(item) : NotFound();
        }
        
        [Authorize]
        [HttpPost("mycart/decrease/{id}")]
        public async Task<ActionResult<CartItemDto>> DecreaseItemToCart(int id)
        {
            var owner = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            var item = await _cartService.DecreaseItem(id, owner);
            
            return item != null ? Ok(item) : NoContent();
        }
        
        [Authorize]
        [HttpPost("mycart/remove/{id}")]
        public async Task<ActionResult<CartItemDto>> RemoveItemCart(int id)
        {
            var owner = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            var item = await _cartService.RemoveItem(id, owner);
            
            return item != null ? Ok(item) : NotFound();
        }
    }
}
