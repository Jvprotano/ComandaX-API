using ComandaX.Application.DTOs;
using ComandaX.Domain.Entities;

namespace ComandaX.Application.Extensions;

public static class ProductExtension
{
    public static ProductDto AsDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Price.AsMoney(),
            product.Code,
            product.NeedPreparation,
            product.ProductCategoryId);
    }

}
