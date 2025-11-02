using ComandaX.Application.DTOs;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.CreateProductCategory;

public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, ProductCategoryDto>
{
    private readonly IProductCategoryRepository _repository;

    public CreateProductCategoryCommandHandler(IProductCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductCategoryDto> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategoryRequest = new ProductCategory(request.Name, request.Icon);

        var productCategory = await _repository.AddAsync(productCategoryRequest);

        return new ProductCategoryDto(productCategory.Id, productCategory.Name);
    }
}
