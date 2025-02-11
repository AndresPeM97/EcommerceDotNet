namespace Ecommerce.DTOs;

public class ProductInsertDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public IFormFile imageFile { get; set; }
}