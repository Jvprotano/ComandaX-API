using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class OrderProductRepository(AppDbContext _context) : IOrderProductRepository
{
    public async Task<IList<OrderProduct>> GetByIdsAsync(IReadOnlyList<Guid> ids)
    {
        return await _context.OrderProducts.Where(op => ids.Contains(op.Id)).ToListAsync();
    }
}
