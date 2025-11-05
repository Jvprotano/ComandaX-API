using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface IProductRepository
{
    Task<IList<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
}
