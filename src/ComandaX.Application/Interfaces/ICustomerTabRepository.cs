using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface ICustomerTabRepository
{
    Task<CustomerTab> CreateAsync(CustomerTab tab);
    Task<CustomerTab?> GetByIdAsync(Guid id);
    Task<IQueryable<CustomerTab>> GetAllAsync();
}
