using ComandaX.Application.DTOs;
using ComandaX.WebAPI.GraphQL.Resolvers;

namespace ComandaX.WebAPI.GraphQL.Types;

public class OrderType : ObjectType<OrderDto>
{
    protected override void Configure(IObjectTypeDescriptor<OrderDto> descriptor)
    {
        descriptor.Field(o => o.Id).Type<NonNullType<IdType>>();
        descriptor.Field(o => o.Status).Type<StringType>();
        descriptor.Field(o => o.Code).Type<IntType>();
    }
}