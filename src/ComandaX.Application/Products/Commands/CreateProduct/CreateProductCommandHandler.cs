using ComandaX.Application.DTOs;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productRequest = new Product(request.Name, request.Price);

        var product = await _repository.AddProductAsync(productRequest);

        return new ProductDto(product.Id, product.Name, product.Price, product.Code);
    }
}

