using ComandaX.Application.DTOs;
using ComandaX.WebAPI.GraphQL.DataLoaders;

namespace ComandaX.WebAPI.GraphQL.Resolvers;

public class ProductResolvers
{
    public async Task<ProductCategoryDto?> GetProductCategory(
        [Parent] ProductDto product,
        GetProductCategoryByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        if (product.ProductCategoryId == null)
            return null;

        var productCategory = await dataLoader.LoadAsync(product.ProductCategoryId.Value, cancellationToken);

        if (productCategory == null)
            return null;

        return new(productCategory.Id, productCategory.Name, productCategory.Icon);
    }
}
