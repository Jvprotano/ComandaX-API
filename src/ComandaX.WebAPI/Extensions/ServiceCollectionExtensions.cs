using ComandaX.WebAPI.GraphQL.DataLoaders;
using ComandaX.WebAPI.GraphQL.Mutations;
using ComandaX.WebAPI.GraphQL.Queries;
using ComandaX.WebAPI.GraphQL.Resolvers;
using ComandaX.WebAPI.GraphQL.Types;

namespace ComandaX.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddType<OrderType>()
            .AddType<TableType>()
            .AddType<CustomerTabType>()
            .AddType<ProductCategoryType>()
            .AddType<ProductType>()
            .AddType<OrderProductType>()
            .AddQueryType(d => d.Name("Query"))
            .AddType<ProductQuery>()
            .AddType<TableQuery>()
            .AddType<OrderQuery>()
            .AddType<CustomerTabQuery>()
            .AddType<ProductCategoryQuery>()
            .AddMutationType(d => d.Name("Mutation"))
            .AddType<AuthMutation>()
            .AddType<ProductMutation>()
            .AddType<TableMutation>()
            .AddType<OrderMutation>()
            .AddType<CustomerTabMutation>()
            .AddType<ProductCategoryMutation>()
            .AddDataLoader<GetTableByIdDataLoader>()
            .AddDataLoader<GetProductCategoryByIdDataLoader>()
            .AddDataLoader<GetProductByIdDataLoader>()
            .AddResolver<CustomerTabResolvers>()
            .AddResolver<ProductResolvers>()
            .AddResolver<OrderProductResolvers>()
            .AddProjections()
            .AddFiltering()
            .AddSorting();

        return services;
    }
}

