using ComandaX.Application.DTOs;
using ComandaX.WebAPI.GraphQL.Resolvers;

namespace ComandaX.WebAPI.GraphQL.Types;

public class CustomerTabType : ObjectType<CustomerTabDto>
{
    protected override void Configure(IObjectTypeDescriptor<CustomerTabDto> descriptor)
    {
        descriptor.Field(t => t.Name).Type<NonNullType<IdType>>();
        descriptor.Field(t => t.TableId).Ignore().IsProjected();
        descriptor.Field(t => t.Table).ResolveWith<CustomerTabResolvers>(r => r.GetTable(default!, default!, default!)).Type<TableType>();
    }
}
