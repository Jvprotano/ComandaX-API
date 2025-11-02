using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class OrderRepository(AppDbContext _context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

        return order;
    }

    public Task<IQueryable<Order>> GetAllAsync()
    {
        return Task.FromResult(_context.Orders.AsQueryable());
    }

    public async Task<Order> AddAsync(Order order)
    {
        var maxCode = await GetMaxCodeAsync();
        order.SetCode(maxCode + 1);

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        return _context.SaveChangesAsync();
    }

    private async Task<int> GetMaxCodeAsync()
    {
        var maxCode = await _context.Orders.AnyAsync() ? _context.Orders.Max(p => p.Code) : 0;
        return maxCode;
    }
}