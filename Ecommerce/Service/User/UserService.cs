using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.DTOs;
using Ecommerce.Models;
using Ecommerce.Repository;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Service;

public class UserService : IUserService<UserDto, UserInsertDto, UserUpdateDto>, IUserLoginService<UserLoginDto>
{
    private IUserRepository<User> _userRepository;
    private IConfiguration _config;

    public UserService(IUserRepository<User> userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }
    
    public Task<IEnumerable<UserDto>> Get()
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> Add(UserInsertDto TIEntityInsertDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> Update(UserUpdateDto TUEntityUpdateDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public bool Validate(UserInsertDto TIEntityInsertDto)
    {
        throw new NotImplementedException();
    }

    public bool Validate(UserUpdateDto TUEntityUpdateDto)
    {
        throw new NotImplementedException();
    }

    public string Authenticate(UserLoginDto userLoginDto)
    {
        // var usuarios = await _userRepository.Get();
        var autheticate = _userRepository.Search(user =>
            user.Email == userLoginDto.Email && user.Password == userLoginDto.Password);

        if (autheticate.Count() > 0)
        {
            var user = GenerateJwtToken(autheticate.First().Email, autheticate.First().Password);
            return user;
        }
        return null;
    }
    
    private string GenerateJwtToken(string username, string password)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Sub, password),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}