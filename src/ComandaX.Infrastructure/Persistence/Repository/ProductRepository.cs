using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence.Repository;

public class ProductRepository : IProductRepository
{
    public Task<Product> AddProductAsync(Product product) 
        => Task.FromResult<Product>(DbFake.AddProduct(product));

    public Task<IList<Product>> GetAllAsync() 
        => Task.FromResult<IList<Product>>(DbFake.GetProducts());

    public Task<Product?> GetProductByIdAsync(Guid id)
    {
        var product = DbFake.GetProducts().FirstOrDefault(p => p.Id == id);
        return Task.FromResult<Product?>(product);
    }
}
