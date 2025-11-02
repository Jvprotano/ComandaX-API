using ComandaX.Application.DTOs;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.Products.Commands.CreateProduct;

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

        if (request.NeedPreparation)
            productRequest.SetNeedPreparation(true);

        var product = await _repository.AddAsync(productRequest);

        return new ProductDto(product.Id, product.Name, product.Price, product.Code);
    }
}

