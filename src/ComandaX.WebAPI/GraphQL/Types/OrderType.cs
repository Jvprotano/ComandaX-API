using ComandaX.Application.DTOs;
using ComandaX.WebAPI.GraphQL.Resolvers;

namespace ComandaX.WebAPI.GraphQL.Types;

public class OrderType : ObjectType<OrderDto>
{
    protected override void Configure(IObjectTypeDescriptor<OrderDto> descriptor)
    {
        descriptor.Field(o => o.Id).Type<NonNullType<IdType>>().IsProjected();
        descriptor.Field(o => o.Status).Type<StringType>();
        descriptor.Field(o => o.Code).Type<IntType>();

        descriptor
            .Field("products")
            .ResolveWith<OrderResolver>(r => r.GetOrderProductsAsync(default!, default!, default!))
            .Type<ListType<OrderProductType>>();
    }
}