using ComandaX.Application.DTOs;
using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Products.Queries.GetProductById;

public class GetProductByIdHandler(IProductRepository _productRepository) : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(request.Id)
        ?? throw new RecordNotFoundException($"Product with Id {request.Id} not found.");

        return new(product.Id, product.Name, product.Price, product.Code);
    }
}
