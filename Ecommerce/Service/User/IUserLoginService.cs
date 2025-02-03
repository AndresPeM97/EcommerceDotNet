using Ecommerce.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Service;

public interface IUserLoginService<TL>
{
    IEnumerable<IdentityError> Errors { get; set; }
    Task<TokenDto> Authenticate(TL TLEntityLoginDto);
    Task<UserDto> UserGetInfo(string email);
    Task<UserDto> UserUpdateInfo(UserUpdateDto userUpdateDto, string email);
    Task<UserDto> UserRegister(UserInsertDto userInsertDto);
    Task<UserDto> AdminRegister(UserInsertDto userInsertDto);
    Task<UserDto> CustomerRegister(UserInsertDto userInsertDto);
    Task<UserDto> UserDelete(string email);
}