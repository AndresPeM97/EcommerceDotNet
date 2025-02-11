using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Ecommerce.DTOs;
using Ecommerce.Models;
using Ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Service;

public class UserService : IUserLoginService<UserLoginDto>
{
    private IUserRepository<User> _userRepository;
    private IConfiguration _config;
    private IMapper _mapper;
    public IEnumerable<IdentityError> Errors { get; set; }

    public UserService(IUserRepository<User> userRepository, IConfiguration config,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _config = config;
        _mapper = mapper;
    }
    public async Task<TokenDto> Authenticate(UserLoginDto userLoginDto)
    {
        var user = _mapper.Map<User>(userLoginDto);
        var validateResult = await _userRepository.ValidatePass(user);

        if (validateResult != null)
        {
            var token = await GenerateJwtToken(user.Email, validateResult);
            var tokenDto = _mapper.Map<TokenDto>(token);
            tokenDto = _mapper.Map(validateResult, tokenDto);
            return tokenDto;
        }

        return null;
    }

    public async Task<UserDto> UserGetInfo(string email)
    {
        var userFind = await _userRepository.GetUserInfo(email);
        var userRoles = await _userRepository.GetRoles(userFind);

        if (userFind != null)
        {
            var userDto = _mapper.Map<UserDto>(userFind);
            userDto.Roles = userRoles;
            return userDto;
        }
        
        return null;
    }

    public async Task<UserDto> UserUpdateInfo(UserUpdateDto userUpdateDto, string email)
    {
        var user = _mapper.Map<User>(userUpdateDto);
        var userFind = await _userRepository.GetUserInfo(email);
        userFind = _mapper.Map(userUpdateDto, userFind);
        if (userFind != null)
        {
            var result = await _userRepository.UpdateInfo(userFind);
            if (result.Succeeded)
            {
                return _mapper.Map<UserDto>(userFind);
            }
        }
        return null;
    }

    public async Task<UserDto> UserRegister(UserInsertDto userInsertDto)
    {
        var user = _mapper.Map<User>(userInsertDto);
        user.UserName = userInsertDto.Email;
        var result = await _userRepository.Register(user);
        await _userRepository.SetUserRole(user, "User");
        if (result.Errors.Count() > 0)
        {
            Errors = result.Errors;
        }

        return result.Succeeded ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto> AdminRegister(UserInsertDto userInsertDto)
    {
        var user = _mapper.Map<User>(userInsertDto);
        user.UserName = userInsertDto.Email;
        var result = await _userRepository.Register(user);
        await _userRepository.SetUserRole(user, "Admin");
        if (result.Errors.Count() > 0)
        {
            Errors = result.Errors;
        }

        return result.Succeeded ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto> CustomerRegister(UserInsertDto userInsertDto)
    {
        var user = _mapper.Map<User>(userInsertDto);
        user.UserName = userInsertDto.Email;
        var result = await _userRepository.Register(user);
        await _userRepository.SetUserRole(user, "Customer");
        await _userRepository.SetUserRole(user, "User");
        if (result.Errors.Count() > 0)
        {
            Errors = result.Errors;
        }

        return result.Succeeded ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto> UserDelete(string email)
    {
        var validateResult = await _userRepository.GetUserInfo(email);
        if (validateResult == null)
        {
            return null;
        }
        var result = await _userRepository.DeleteUser(validateResult);
        if (result.Errors.Count() > 0)
        {
            Errors = result.Errors;
        }
        return result.Succeeded ? _mapper.Map<UserDto>(validateResult) : null;
    }

    private async Task<string> GenerateJwtToken(string email, User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var userRoles = await _userRepository.GetRoles(user);
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}