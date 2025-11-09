using ComandaX.Application.DTOs;
using ComandaX.WebAPI.GraphQL.Resolvers;

namespace ComandaX.WebAPI.GraphQL.Types;

public class ProductType : ObjectType<ProductDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProductDto> descriptor)
    {
        descriptor.Field<ProductResolvers>(p => p.GetProductCategory(default!, default!, default!)).Type<ProductCategoryType>();
    }
}
