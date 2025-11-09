using ComandaX.Application.DTOs;
using ComandaX.WebAPI.GraphQL.Resolvers;

namespace ComandaX.WebAPI.GraphQL.Types;

public class OrderProductType : ObjectType<OrderProductDto>
{
    protected override void Configure(IObjectTypeDescriptor<OrderProductDto> descriptor)
    {
        descriptor.Field(o => o.ProductId).Type<NonNullType<IdType>>().IsProjected();
        descriptor.Field(o => o.OrderId).Type<NonNullType<IdType>>();
        descriptor.Field(o => o.Quantity).Type<IntType>();
        descriptor.Field(o => o.TotalPrice).Type<DecimalType>();
        descriptor.Field("product").ResolveWith<OrderProductResolvers>(r => r.GetProduct(default!, default!, default!)).Type<ProductType>();
    }
}
