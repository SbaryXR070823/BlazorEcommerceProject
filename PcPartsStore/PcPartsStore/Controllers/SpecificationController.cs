using Microsoft.AspNetCore.Mvc;
using PcPartsStore.Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shared.Models;

[Route("api/[controller]")]
[ApiController]
public class SpecificationController : ControllerBase
{
    private readonly ISpecificationService _specificationService;

    public SpecificationController(ISpecificationService specificationService)
    {
        _specificationService = specificationService;
    }

    // GET: api/Specification
    [HttpGet]
    public async Task<IActionResult> GetSpecifications()
    {
        var specifications = await _specificationService.GetSpecificationsAsync();
        return Ok(specifications);
    }

    // GET: api/Specification/5
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

    // POST: api/Specification
    [HttpPost]
    public async Task<IActionResult> AddSpecification(Specification specification)
    {
        await _specificationService.AddSpecificationAsync(specification);
        return NoContent();
    }

    // PUT: api/Specification/5
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

    // DELETE: api/Specification/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpecification(int id)
    {
        await _specificationService.DeleteSpecificationAsync(id);
        return NoContent();
    }
}