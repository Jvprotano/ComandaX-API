using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.CreateProductCategory;

public class CreateProductCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCategoryCommand, ProductCategoryDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ProductCategoryDto> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategoryRequest = new ProductCategory(request.Name, request.Icon);

        var productCategory = await _unitOfWork.ProductCategories.AddAsync(productCategoryRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return productCategory.AsDto();
    }
}
