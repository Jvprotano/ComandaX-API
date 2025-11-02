
using ComandaX.Application.DTOs;
using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Queries.GetProductCategoryById;

public class GetProductCategoryByIdQueryHandler(IProductCategoryRepository _productCategoryRepository) : IRequestHandler<GetProductCategoryByIdQuery, ProductCategoryDto>
{
    public async Task<ProductCategoryDto> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var productCategory = await _productCategoryRepository.GetByIdAsync(request.Id)
        ?? throw new RecordNotFoundException($"ProductCategory with Id {request.Id} not found.");

        return new(productCategory.Id, productCategory.Name);
    }
}
