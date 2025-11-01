using ComandaX.Application.CustomerTabs.Commands.CreateCustomerTab;
using ComandaX.WebAPI.GraphQL.Mutations;
using ComandaX.WebAPI.GraphQL.Queries;
using ComandaX.WebAPI.GraphQL.Types;

namespace ComandaX.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddQueryType(d => d.Name("Query"))
            .AddType<ProductQuery>()
            .AddType<TableQuery>()
            .AddType<OrderQuery>()
            .AddType<CustomerTabQuery>()
            .AddMutationType(d => d.Name("Mutation"))
            .AddType<OrderType>()
            .AddType<TableType>()
            .AddType<ProductMutation>()
            .AddType<TableMutation>()
            .AddType<OrderMutation>()
            .AddType<CustomerTabMutation>()
            .AddProjections()
            .AddFiltering()
            .AddSorting();

        return services;
    }
}

