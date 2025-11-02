using ComandaX.Application.DTOs;

namespace ComandaX.WebAPI.GraphQL.Types;

public class OrderProductType : ObjectType<OrderProductDto>
{
    protected override void Configure(IObjectTypeDescriptor<OrderProductDto> descriptor)
    {
        descriptor.Field(o => o.ProductName).Type<StringType>();
        descriptor.Field(o => o.UnitPrice).Type<DecimalType>();
        descriptor.Field(o => o.Quantity).Type<IntType>();
        descriptor.Field(o => o.TotalPrice).Type<DecimalType>();
    }
}
