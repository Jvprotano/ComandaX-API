using ComandaX.Application.DTOs;
using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.GraphQL.Types;

public class TableType : ObjectType<TableDto>
{
    protected override void Configure(IObjectTypeDescriptor<TableDto> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<IdType>>();
        descriptor.Field(t => t.Code).Type<IntType>();
    }
}