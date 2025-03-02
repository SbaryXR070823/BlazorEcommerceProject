namespace Shared.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public string ImageBase64 { get; set; }  
    public ICollection<Specification> Specifications { get; set; }
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
