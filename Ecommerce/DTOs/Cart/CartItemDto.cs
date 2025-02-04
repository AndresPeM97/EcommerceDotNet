using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.DTOs;

public class CartItemDto
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }
}