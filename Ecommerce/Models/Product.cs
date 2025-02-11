using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    
    [Column(TypeName = "decimal(2,1)")]
    public decimal? Rate { get; set; }
    
    [ForeignKey("UserId")]
    virtual public User User { get; set; }
    public string UserId  { get; set; }
}