using Microsoft.EntityFrameworkCore;
using PcPartsStore.Data;
using PcPartsStore.Repository;
using PcPartsStore;
using Shared.Models;
namespace PcPartsStore.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IRepository<Category> Categories { get; }
    public IRepository<Product> Products { get; }
    public IRepository<Specification> Specifications { get; }
    public IRepository<CartItem> CartItems { get; }
    public IRepository<Order> Orders { get; }
    public IRepository<OrderItem> OrderItems { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Categories = new Repository<Category>(_context);
        Products = new Repository<Product>(_context);
        Specifications = new Repository<Specification>(_context);
        CartItems = new Repository<CartItem>(_context);
        Orders = new Repository<Order>(_context);
        OrderItems = new Repository<OrderItem>(_context);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}