using PcPartsStore.Repository;
using Shared.Models;

namespace PcPartsStore.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<Category> Categories { get; }
        IRepository<Product> Products { get; }
        IRepository<Specification> Specifications { get; }
        IRepository<CartItem> CartItems { get; }
        IRepository<Order> Orders { get; }

        Task SaveChangesAsync();
    }
}
