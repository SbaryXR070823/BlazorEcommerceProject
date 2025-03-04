using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PcPartsStore.Services;
using PcPartsStore.Services.Interfaces;
using Shared.Models;
using Shared.Requests;
using System.Security.Claims;

namespace PcPartsStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartItemsController : ControllerBase
{
    private readonly ICartItemsService _cartItemsService;
    private readonly UserManager<Data.ApplicationUser> _userManager;

    public CartItemsController(ICartItemsService cartItemsService, UserManager<Data.ApplicationUser> userManager)
    {
        _cartItemsService = cartItemsService;
        _userManager = userManager; 
    }

    [HttpGet]
    public async Task<IActionResult> GetCartItems()
    {
        var cartItems = await _cartItemsService.GetCartItemsAsync();
        return Ok(cartItems);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCartItemsCount()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = await _cartItemsService.GetCountOfCartITems(userId);
            return Ok(count);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetCartItemsCount: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCartItemById(int id)
    {
        var cartItem = await _cartItemsService.GetCartItemsByIdAsync(id);
        if (cartItem == null)
        {
            return NotFound();
        }
        return Ok(cartItem);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetCartItemsByUserId(string userId)
    {
        var cartItems = await _cartItemsService.GetCartItemsByUserIdAsync(userId);
        return Ok(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> AddCartItem([FromBody] CartItemRequest cartItem)
    {
        if (cartItem == null)
        {
            return BadRequest("Invalid cart item data.");
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var newCartItemToAdd = new CartItem
        {
            UserId = userId,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity
        };
        await _cartItemsService.AddCartItemAsync(newCartItemToAdd);
        return CreatedAtAction(nameof(GetCartItemById), new { id = newCartItemToAdd.Id }, newCartItemToAdd);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItemRequest cartItem)
    {
        if (cartItem == null)
        {
            return BadRequest("Invalid cart item data.");
        }

        var existingCartItem = await _cartItemsService.GetCartItemsByIdAsync(id);
        if (existingCartItem == null)
        {
            return NotFound();
        }

        existingCartItem.ProductId = cartItem.ProductId;
        existingCartItem.Quantity = cartItem.Quantity;

        await _cartItemsService.UpdateCartItemAsync(existingCartItem);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCartItem(int id)
    {
        var existingCartItem = await _cartItemsService.GetCartItemsByIdAsync(id);
        if (existingCartItem == null)
        {
            return NotFound();
        }

        await _cartItemsService.DeleteCartItemAsync(id);
        return NoContent();
    }

    [HttpGet("get/{productId}")]
    public async Task<IActionResult> GetCartItemByProductIdForUser(int productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cartItem = await _cartItemsService.GetCartItemByProductIdForUser(productId, userId);
        return Ok(cartItem);
    }
}
