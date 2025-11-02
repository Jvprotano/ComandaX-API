using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Products.Queries.GetProducts;

public class GetProductsQueryHandler(IProductRepository _productRepository) : IRequestHandler<GetProductsQuery, IList<ProductDto>>
{
    public async Task<IList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync();
        return [.. products.Select(p => p.AsDto())];
    }
}
