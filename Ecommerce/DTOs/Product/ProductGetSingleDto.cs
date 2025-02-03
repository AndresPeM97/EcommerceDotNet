using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.DTOs;

public class ProductGetSingleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    
    [Column(TypeName = "decimal(2,1)")]
    public decimal Rate { get; set; }
}