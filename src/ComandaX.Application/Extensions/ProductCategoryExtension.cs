using ComandaX.Application.DTOs;
using ComandaX.Domain.Entities;

namespace ComandaX.Application.Extensions;

public static class ProductCategoryExtension
{
    public static ProductCategoryDto AsDto(this ProductCategory productCategory)
    {
        return new ProductCategoryDto(
            productCategory.Id,
            productCategory.Name,
            productCategory.Icon);
    }
}
