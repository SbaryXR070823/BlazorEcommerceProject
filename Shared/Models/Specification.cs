namespace Shared.Models
{
    public class Specification
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
