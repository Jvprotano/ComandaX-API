using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class OrderRepository(AppDbContext _context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order;
    }

    public async Task<IList<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(o => o.Product)
            .ToListAsync();
    }

    public async Task<Order> AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        return order;
    }

    public Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        return Task.CompletedTask;
    }

    public async Task<IList<Order>> GetByIdsAsync(IReadOnlyList<Guid> ids)
    {
        return await _context.Orders
            .Include(o => o.OrderProducts)
            .Where(o => ids.Contains(o.Id))
            .ToListAsync();
    }
}