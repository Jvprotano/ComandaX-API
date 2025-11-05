using ComandaX.Application.DTOs;

namespace ComandaX.WebAPI.GraphQL.Types;

public class OrderProductType : ObjectType<OrderProductDto>
{
    protected override void Configure(IObjectTypeDescriptor<OrderProductDto> descriptor)
    {
        descriptor.Field(o => o.ProductId).Type<NonNullType<IdType>>();
        descriptor.Field(o => o.Quantity).Type<IntType>();
        descriptor.Field(o => o.TotalPrice).Type<DecimalType>();
    }
}
