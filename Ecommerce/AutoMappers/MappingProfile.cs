using AutoMapper;
using Ecommerce.DTOs;
using Ecommerce.Models;

namespace Ecommerce.AutoMappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<string, TokenDto>().ForMember(dto => dto.Token,
            m => m.MapFrom(b => b));
        
        CreateMap<UserInsertDto, User>().ForMember(u => u.PasswordHash,
            m => m.MapFrom(dto => dto.Password));

        CreateMap<User, UserDto>();
        
        CreateMap<UserLoginDto, User>().ForMember(u => u.PasswordHash,
            m => m.MapFrom(dto => dto.Password));

        CreateMap<User, TokenDto>();

        CreateMap<UserGetDto, User>();

        CreateMap<UserUpdateDto, User>();
        
        CreateMap<Product, ProductGetSingleDto>();
        CreateMap<Product, ProductGetListDto>();

        CreateMap<ProductInsertDto, Product>();
        CreateMap<ProductChangeDto, Product>();

        CreateMap<Cart, CartItemDto>()
            .ForMember(dto => dto.Name,
            m => m.MapFrom(i => i.Product.Name))
            .ForMember(dto => dto.ProductId,
                m => m.MapFrom(i => i.Product.Id))
            .ForMember(dto => dto.Price,
            m => m.MapFrom(i => i.Product.Price));
    }
}