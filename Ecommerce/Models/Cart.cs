using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models;

public class Cart
{    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    virtual public User User { get; set; }
    
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    virtual public Product Product { get; set; }
    
    public int Quantity { get; set; }
    
    
    
}