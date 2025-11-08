using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productRequest = new Product(request.Name, request.Price);

        if (request.NeedPreparation)
            productRequest.SetNeedPreparation(request.NeedPreparation);

        if (request.ProductCategoryId.HasValue)
            productRequest.SetProductCategory(request.ProductCategoryId.Value);

        var product = await _unitOfWork.Products.AddAsync(productRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.AsDto();
    }
}

