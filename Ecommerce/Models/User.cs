using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Country { get; set; }
}