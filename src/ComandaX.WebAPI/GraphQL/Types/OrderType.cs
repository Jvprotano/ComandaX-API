using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.GraphQL.Types;

public class OrderType : ObjectType<Order>
{
    protected override void Configure(IObjectTypeDescriptor<Order> descriptor)
    {
        descriptor.Field(o => o.Id).Type<NonNullType<IdType>>();
        descriptor.Field(o => o.Table).Type<TableType>().UseProjection();
        descriptor.Field(o => o.OrderProducts).Type<ListType<ProductType>>().UseProjection();
    }
}