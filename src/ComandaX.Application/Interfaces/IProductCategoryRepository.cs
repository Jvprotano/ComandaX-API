using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface IProductCategoryRepository
{
    Task<IList<ProductCategory>> GetAllAsync();
    Task<ProductCategory?> GetByIdAsync(Guid id);
    Task<ProductCategory> AddAsync(ProductCategory productCategory);
    Task<IReadOnlyList<ProductCategory>> GetByIdsAsync(IReadOnlyList<Guid> keys);
}
