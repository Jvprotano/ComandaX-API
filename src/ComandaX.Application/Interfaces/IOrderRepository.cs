using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IList<Order>> GetAllAsync();
    Task<Order> CreateAsync(Order order);
    Task UpdateAsync(Order order);
}
