using Microsoft.AspNetCore.Mvc;
using PcPartsStore.Services.Interfaces;
using Shared.Dto;
using Shared.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetProducts(
    [FromQuery] string name = null,
    [FromQuery] string category = null,
    [FromQuery] decimal? minPrice = null,
    [FromQuery] decimal? maxPrice = null,
    [FromQuery] Dictionary<string, string> specifications = null)
    {
        if (minPrice < 0 || maxPrice < 0)
        {
            return BadRequest("Price values cannot be negative.");
        }
        if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
        {
            return BadRequest("Minimum price cannot be greater than maximum price.");
        }
        var cleanSpecifications = new Dictionary<string, string>();

        if (specifications != null)
        {
            foreach (var spec in specifications)
            {
                if (spec.Key.Equals("Name", StringComparison.OrdinalIgnoreCase) ||
                    (name != null && spec.Key.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                cleanSpecifications[spec.Key] = spec.Value;
            }
        }

        var filters = new ProductFilters
        {
            Name = name,
            Category = category,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Specifications = cleanSpecifications
        };

        var products = await _productService.GetProductsAsync(filters);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductCreateUpdateDto productDto)
    {
        if (productDto == null)
        {
            return BadRequest("Invalid product data.");
        }

        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            CategoryId = productDto.CategoryId,
            ImageBase64 = productDto.ImageBase64
        };

        await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductCreateUpdateDto productDto)
    {
        if (productDto == null || id != productDto.Id)
        {
            return BadRequest("Invalid product data.");
        }

        var existingProduct = await _productService.GetProductByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        existingProduct.Name = productDto.Name;
        existingProduct.Description = productDto.Description;
        existingProduct.Price = productDto.Price;
        existingProduct.CategoryId = productDto.CategoryId;
        existingProduct.ImageBase64 = productDto.ImageBase64;

        await _productService.UpdateProductAsync(existingProduct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var existingProduct = await _productService.GetProductByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}