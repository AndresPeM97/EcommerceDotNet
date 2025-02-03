using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs;

public class UserLoginDto
{
    [EmailAddress]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}