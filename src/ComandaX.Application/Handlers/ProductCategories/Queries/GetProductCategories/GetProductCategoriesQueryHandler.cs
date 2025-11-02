
using ComandaX.Application.DTOs;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Queries.GetProductCategories;

public class GetProductCategoriesQueryHandler(IProductCategoryRepository _productCategoryRepository) : IRequestHandler<GetProductCategoriesQuery, IList<ProductCategoryDto>>
{
    public async Task<IList<ProductCategoryDto>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var productCategories = await _productCategoryRepository.GetAllAsync();
        return productCategories.Select(pc => new ProductCategoryDto(pc.Id, pc.Name)).ToList();
    }
}
