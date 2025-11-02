using ComandaX.Application.DTOs;

namespace ComandaX.WebAPI.GraphQL.Types;

public class ProductCategoryType : ObjectType<ProductCategoryDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProductCategoryDto> descriptor)
    {
        descriptor.Field(p => p.Id).Type<NonNullType<IdType>>();
        descriptor.Field(p => p.Name).Type<StringType>();
        descriptor.Field(p => p.Icon).Type<StringType>();
    }
}
