using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using ComandaX.Domain.Enums;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class SubscriptionRepository(AppDbContext _context) : ISubscriptionRepository
{
    public async Task<Subscription?> GetByIdAsync(Guid id)
    {
        return await _context.Subscriptions
            .Include(s => s.Tenant)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Subscription?> GetByTenantIdAsync(Guid tenantId)
    {
        return await _context.Subscriptions
            .Include(s => s.Tenant)
            .FirstOrDefaultAsync(s => s.TenantId == tenantId);
    }

    public async Task<IEnumerable<Subscription>> GetExpiringSoonAsync(int daysThreshold = 7)
    {
        var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);

        return await _context.Subscriptions
            .Include(s => s.Tenant)
            .Where(s => (s.Status == SubscriptionStatusEnum.Trial || s.Status == SubscriptionStatusEnum.Active)
                        && s.EndDate <= thresholdDate
                        && s.EndDate > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<IEnumerable<Subscription>> GetExpiredSubscriptionsAsync()
    {
        return await _context.Subscriptions
            .Include(s => s.Tenant)
            .Where(s => (s.Status == SubscriptionStatusEnum.Trial || s.Status == SubscriptionStatusEnum.Active)
                        && s.EndDate <= DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task AddAsync(Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
    }

    public void Update(Subscription subscription)
    {
        _context.Subscriptions.Update(subscription);
    }
}

