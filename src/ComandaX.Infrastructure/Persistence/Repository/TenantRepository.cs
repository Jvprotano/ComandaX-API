using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class TenantRepository(AppDbContext _context) : ITenantRepository
{
    public async Task<Tenant?> GetByIdAsync(Guid id)
    {
        return await _context.Tenants.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tenant> AddAsync(Tenant tenant)
    {
        await _context.Tenants.AddAsync(tenant);
        return tenant;
    }

    public Task UpdateAsync(Tenant tenant)
    {
        _context.Tenants.Update(tenant);
        return Task.CompletedTask;
    }

    public async Task<IList<Tenant>> GetAllAsync()
    {
        return await _context.Tenants.ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Tenants.AnyAsync(t => t.Id == id);
    }
}

