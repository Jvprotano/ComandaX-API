using ComandaX.Application.DTOs;
using ComandaX.WebAPI.GraphQL.Resolvers;

namespace ComandaX.WebAPI.GraphQL.Types;

public class TabType : ObjectType<TabDto>
{
    protected override void Configure(IObjectTypeDescriptor<TabDto> descriptor)
    {
        descriptor.Field(t => t.Name).Type<NonNullType<IdType>>();
        descriptor.Field(t => t.TableId).IsProjected();
        descriptor.Field(t => t.Table).ResolveWith<TabResolvers>(r => r.GetTable(default!, default!, default!)).Type<TableType>();
    }
}
