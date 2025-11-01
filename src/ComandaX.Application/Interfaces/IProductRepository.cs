using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface IProductRepository
{
    Task<IList<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> AddProductAsync(Product product);
    Task<int> GetMaxCodeAsync();
}
