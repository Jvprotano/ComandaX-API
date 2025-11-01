using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.GraphQL.Types;

public class ProductType : ObjectType<Product>
{
    protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
    {
        descriptor.Field(p => p.Id).Type<NonNullType<IdType>>();
        descriptor.Field(p => p.Name).Type<StringType>();
        descriptor.Field(p => p.Price).Type<DecimalType>();
        descriptor.Field(p => p.Code).Type<IntType>();
    }
}
