using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(Guid id);
    Task<Tenant> AddAsync(Tenant tenant);
    Task UpdateAsync(Tenant tenant);
    Task<IList<Tenant>> GetAllAsync();
    Task<bool> ExistsAsync(Guid id);
}

