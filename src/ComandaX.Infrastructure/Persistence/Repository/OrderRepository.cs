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

    public Task<Order> CreateAsync(Order order)
    {
        _context.Orders.Add(order);
        return _context.SaveChangesAsync().ContinueWith(_ => order);
    }

    public Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        return _context.SaveChangesAsync();
    }
}