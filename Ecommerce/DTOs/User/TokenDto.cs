using System.Collections;

namespace Ecommerce.DTOs;

public class TokenDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public IEnumerable<string> Role { get; set; }
    public string Token { get; set; }
}