using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs;

public class UserUpdateDto
{
    public string Username { get; set; }
    public string Surname { get; set; }
    public string Country { get; set; }
    [DataType(DataType.PhoneNumber)]
    public decimal PhoneNumber { get; set; }
}