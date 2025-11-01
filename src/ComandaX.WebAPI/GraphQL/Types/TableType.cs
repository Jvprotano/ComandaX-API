using ComandaX.Domain.Entities;

namespace ComandaX.WebAPI.GraphQL.Types;

public class TableType : ObjectType<Table>
{
    protected override void Configure(IObjectTypeDescriptor<Table> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<IdType>>();
    }
}