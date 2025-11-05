using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class ProductCategoryRepository(AppDbContext _context) : IProductCategoryRepository
{
    public async Task<ProductCategory> AddAsync(ProductCategory productCategory)
    {
        await _context.AddAsync(productCategory);
        await _context.SaveChangesAsync();
        return productCategory;
    }

    public async Task<IList<ProductCategory>> GetAllAsync()
    {
        return await _context.ProductCategories.ToListAsync();
    }

    public async Task<ProductCategory?> GetByIdAsync(Guid id)
    {
        return await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == id);
    }

    public async Task<IReadOnlyList<ProductCategory>> GetByIdsAsync(IReadOnlyList<Guid> keys)
    {
        return await _context.ProductCategories.Where(pc => keys.Contains(pc.Id)).ToListAsync();
    }

    public async Task UpdateAsync(ProductCategory productCategory)
    {
        _context.ProductCategories.Update(productCategory);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductCategory productCategory)
    {
        _context.ProductCategories.Remove(productCategory);
        await _context.SaveChangesAsync();
    }
}
