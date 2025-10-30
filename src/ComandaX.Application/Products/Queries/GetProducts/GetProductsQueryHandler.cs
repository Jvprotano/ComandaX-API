using ComandaX.Application.DTOs;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler(IProductRepository _productRepository) : IRequestHandler<GetProductsQuery, IList<ProductDto>>
{
    public async Task<IList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync();
        return [.. products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Code))];
    }
}
