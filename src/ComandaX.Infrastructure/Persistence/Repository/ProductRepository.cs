using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class ProductRepository(AppDbContext _context) : IProductRepository
{
    public async Task<Product> AddProductAsync(Product product)
    {
        var maxCode = await GetMaxCodeAsync();

        if (product.Code != maxCode + 1)
            product.SetCode(maxCode + 1);

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

    private async Task<int> GetMaxCodeAsync()
    {
        var maxCode = await _context.Products.AnyAsync() ? _context.Products.Max(p => p.Code) : 0;
        return maxCode;
    }
}
