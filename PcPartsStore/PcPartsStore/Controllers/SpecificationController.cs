using Microsoft.AspNetCore.Mvc;
using PcPartsStore.Services;
using PcPartsStore.Services.Interfaces;
using Shared.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class SpecificationController : ControllerBase
{
    private readonly ISpecificationService _specificationService;

    public SpecificationController(ISpecificationService specificationService)
    {
        _specificationService = specificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSpecifications()
    {
        var specifications = await _specificationService.GetSpecificationsAsync();
        return Ok(specifications);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpecificationById(int id)
    {
        var specification = await _specificationService.GetSpecificationByIdAsync(id);
        if (specification == null)
        {
            return NotFound();
        }
        return Ok(specification);
    }

    [HttpPost]
    public async Task<IActionResult> AddSpecification(Specification specification)
    {
        await _specificationService.AddSpecificationAsync(specification);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSpecification(int id, Specification specification)
    {
        if (id != specification.Id)
        {
            return BadRequest();
        }
        await _specificationService.UpdateSpecificationAsync(specification);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpecification(int id)
    {
        await _specificationService.DeleteSpecificationAsync(id);
        return NoContent();
    }

    [HttpGet("get/{productId}")]
    public async Task<IActionResult> GetSpecificationByProductId(int productId)
    {
        var specifications = await _specificationService.GetSpecificationsByProductIdAsync(productId);
        return Ok(specifications);
    }

    [HttpPost("getbyproductids")]
    public async Task<IActionResult> GetSpecificationsByProductIds([FromBody] List<int> productIds)
    {
        var specifications = await _specificationService.GetSpecificationsByProductIdsAsync(productIds);
        return Ok(specifications);
    }

    [HttpPost("getgroupedbyproductids")]
    public async Task<IActionResult> GetGroupedSpecificationsForProducts([FromBody] List<int> productIds)
    {
        var groupedSpecifications = await _specificationService.GetGroupedSpecificationsForProductsAsync(productIds);
        return Ok(groupedSpecifications);
    }
}