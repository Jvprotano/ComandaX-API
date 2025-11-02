using ComandaX.Application.DTOs;
using ComandaX.WebAPI.GraphQL.Resolvers;

namespace ComandaX.WebAPI.GraphQL.Types;

public class ProductType : ObjectType<ProductDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProductDto> descriptor)
    {
        descriptor.Field(p => p.Id).Type<NonNullType<IdType>>();
        descriptor.Field(p => p.Name).Type<StringType>();
        descriptor.Field(p => p.Price).Type<DecimalType>();
        descriptor.Field(p => p.Code).Type<IntType>();
        descriptor.Field<ProductResolvers>(p => p.GetProductCategory(default!, default!, default!)).Type<ProductCategoryType>();
    }
}
