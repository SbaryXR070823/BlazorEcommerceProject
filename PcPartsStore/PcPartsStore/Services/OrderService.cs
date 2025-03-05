using PcPartsStore.Services.Interfaces;
using PcPartsStore.UnitOfWork;
using Shared.Models;

namespace PcPartsStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return (await _unitOfWork.Orders.GetAllAsync()).ToList();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _unitOfWork.Orders.GetByIdAsync(id);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order);
        }

    }
}
