using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class CustomerTabRepository(AppDbContext _context) : ICustomerTabRepository
{
    public async Task<CustomerTab> CreateAsync(CustomerTab tab)
    {
        await _context.CustomerTabs.AddAsync(tab);
        return tab;
    }

    public async Task<IQueryable<CustomerTab>> GetAllAsync()
    {
        return await Task.FromResult(
            _context.CustomerTabs);
    }

    public async Task<CustomerTab?> GetByIdAsync(Guid id)
    {
        return await _context.CustomerTabs.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<CustomerTab?> GetByIdWithOrdersAsync(Guid id)
    {
        return await _context.CustomerTabs
            .Include(t => t.Orders)
            .ThenInclude(o => o.OrderProducts)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public Task UpdateAsync(CustomerTab tab)
    {
        _context.CustomerTabs.Update(tab);
        return Task.CompletedTask;
    }
}
