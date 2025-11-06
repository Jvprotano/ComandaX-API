using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class ProductRepository(AppDbContext _context) : IProductRepository
{
    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<IList<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IList<Product>> GetByIdsAsync(IReadOnlyList<Guid> ids)
    {
        return await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
}
