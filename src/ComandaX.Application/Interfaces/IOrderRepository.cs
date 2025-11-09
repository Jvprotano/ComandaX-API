using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task<IList<Order>> GetByIdsAsync(IReadOnlyList<Guid> ids);
    Task<IQueryable<Order>> GetAllAsync();
    IQueryable<Order> GetAll();
    Task<IList<Order>> GetByCustomerTabIdAsync(Guid customerTabId);
}
