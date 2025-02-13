using System.Security.Claims;
using Ecommerce.DTOs;
using Ecommerce.Models;
using Ecommerce.Repository;
using Ecommerce.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productService.GetProducts();
            
            return products.Count() > 0 ? Ok(products) : NotFound("No se encontraron productos");
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductGetSingleDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            
            return product == null ? NotFound("No se encontro el producto") : Ok(product);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<ActionResult<ProductGetSingleDto>> AddProduct(ProductInsertDto productInsertDto)
        {
            var owner = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var product = await _productService.AddProduct(productInsertDto, owner, productInsertDto.imageFile);
            
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductGetSingleDto>> UpdateProduct(ProductChangeDto productChangeDto, int id)
        {
            var product = await _productService.UpdateProduct(productChangeDto, id);
            
            return product == null ? NotFound("No se encontro el producto") : Ok(product);
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductGetSingleDto>> DeleteProduct(int id)
        {
            var product = await _productService.DeleteProduct(id);
            
            return product != null ? Ok(product) : NotFound();
        }
        
        [Authorize(Roles = "Customer")]
        [HttpGet("myproducts")]
        public async Task<ActionResult<IEnumerable<ProductGetListDto>>> GetMyProductsList()
        {
            var owner = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var myproducts = await _productService.SearchProducts(owner);
            
            return myproducts.Count() > 0 ? Ok(myproducts) : NotFound("No se encontraron productos");
        }
        
        [Authorize(Roles = "Customer")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadProductImage(int productId, IFormFile imageFile)
        {
            var imageResult = await _productService.UploadImage(productId, imageFile);

            return imageResult != null ? Ok(imageResult) : NotFound();
        }

        
    }
}
