using AutoMapper;
using Ecommerce.DTOs;
using Ecommerce.Models;
using Ecommerce.Repository;

namespace Ecommerce.Service;

public class ProductService : IProductService
{
    private IProductRepository<Product> _productRepository;
    private IUserRepository<User> _userRepository;
    private IMapper _mapper;

    public ProductService(IProductRepository<Product> productRepository,
        IMapper mapper, IUserRepository<User> userRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<ProductGetSingleDto> GetProductById(int id)
    {
        var product = await _productRepository.GetProductById(id);

        if (product != null)
        {
            return _mapper.Map<ProductGetSingleDto>(product);
        }
        return null;
    }

    public async Task<IEnumerable<ProductGetListDto>> GetProducts()
    {
        var products = await _productRepository.GetProducts();
        
        return products.Select(p => _mapper.Map<ProductGetListDto>(p));
    }

    public async Task<ProductGetSingleDto> AddProduct(ProductInsertDto productInsertDto, string owner, IFormFile imageFile)
    {
        var productOwner = await _userRepository.GetUserInfo(owner);
        
        if(productOwner == null){return null;}
        
        var product = _mapper.Map<Product>(productInsertDto);
        var productFind = _productRepository.Search(p => p.Name == product.Name).FirstOrDefault();

        if (productFind == null)
        {
            product.UserId = productOwner.Id;
            await _productRepository.AddProduct(product);
            await _productRepository.SaveChanges();
            
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
        
            var filePath = Path.Combine(uploadsFolder, product.Name + ".jpg");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            
            return _mapper.Map<ProductGetSingleDto>(product);
        }

        return null;
    }

    public async Task<ProductGetSingleDto> UpdateProduct(ProductChangeDto productChangeDto, int id)
    {
        var productFind = await _productRepository.GetProductById(id);

        if (productFind != null)
        {
            var product = _mapper.Map(productChangeDto, productFind);
            _productRepository.UpdateProduct(product);
            await _productRepository.SaveChanges();

            return _mapper.Map<ProductGetSingleDto>(product);
        }

        return null;
    }

    public async Task<ProductGetSingleDto> DeleteProduct(int id)
    {
        var product = await _productRepository.GetProductById(id);

        if (product != null)
        {
            _productRepository.DeleteProduct(product);
            await _productRepository.SaveChanges();
            
            return _mapper.Map<ProductGetSingleDto>(product);
        }
        return null;
    }

    public async Task<IEnumerable<ProductGetListDto>> SearchProducts(string owner)
    {
        var productOwner = await _userRepository.GetUserInfo(owner);
        
        var products = _productRepository.Search(p => p.UserId == productOwner.Id);
        return products.Select(p => _mapper.Map<ProductGetListDto>(p));
    }

    public async Task<string> UploadImage(int productId, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            return null;
        }

        var product = await _productRepository.GetProductById(productId);
        if (product == null)
        {
            return null;
        }

        // Guardar la imagen en el servidor
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }
        
        var filePath = Path.Combine(uploadsFolder, product.Name + ".jpg");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }
        
        return filePath;
    }
}