using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs;

public class UserInsertDto
{
    public string Username { get; set; }
    public string Surname { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Country { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}