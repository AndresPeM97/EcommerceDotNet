using Ecommerce.DTOs;
using Ecommerce.Models;

namespace Ecommerce.Service;

public interface IProductService
{
    Task<ProductGetSingleDto> GetProductById(int id);
    Task<IEnumerable<ProductGetListDto>> GetProducts();
    Task<ProductGetSingleDto> AddProduct(ProductInsertDto entity, string owner, IFormFile imageFile);
    Task<ProductGetSingleDto> UpdateProduct(ProductChangeDto entity, int id);
    Task<ProductGetSingleDto> DeleteProduct(int id);
    Task<IEnumerable<ProductGetListDto>> SearchProducts(string owner);
    Task<string> UploadImage(int productId, IFormFile imageFile);
}