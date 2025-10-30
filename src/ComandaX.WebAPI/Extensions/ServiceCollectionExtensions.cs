using ComandaX.WebAPI.GraphQL.Queries;
using HotChocolate.Data;

namespace ComandaX.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
    {
        // services
        //     .AddGraphQLServer()
        //     .AddQueryType(d => d.Name("Query"))
        //     .AddType<ProductQuery>()
        //     // .AddMutationType(d => d.Name("Mutation"))
        //     // .AddType<ProductMutation>()
        //     .AddProjections()
        //     .AddFiltering()
        //     .AddSorting();

        return services;
    }
}

