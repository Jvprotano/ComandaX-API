using ComandaX.WebAPI.GraphQL.DataLoaders;
using ComandaX.WebAPI.GraphQL.Filters;
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
            .AddType<TableType>()
            .AddType<CustomerTabType>()
            .AddType<ProductCategoryType>()
            .AddType<ProductType>()
            .AddType<OrderProductType>()
            .AddType<OrderType>()
            .AddQueryType(d => d.Name("Query"))
            .AddType<ProductQuery>()
            .AddType<TableQuery>()
            .AddType<OrderQuery>()
            .AddType<CustomerTabQuery>()
            .AddType<ProductCategoryQuery>()
            .AddType<SubscriptionQuery>()
            .AddMutationType(d => d.Name("Mutation"))
            .AddType<AuthMutation>()
            .AddType<ProductMutation>()
            .AddType<TableMutation>()
            .AddType<OrderMutation>()
            .AddType<CustomerTabMutation>()
            .AddType<ProductCategoryMutation>()
            .AddType<SubscriptionMutation>()
            .AddDataLoader<GetTableByIdDataLoader>()
            .AddDataLoader<GetProductCategoryByIdDataLoader>()
            .AddDataLoader<GetOrderProductByOrderIdDataLoader>()
            .AddDataLoader<GetProductByIdDataLoader>()
            .AddDataLoader<GetOrderByCustomerTabIdDataLoader>()
            .AddResolver<CustomerTabResolvers>()
            .AddResolver<ProductResolvers>()
            .AddResolver<OrderProductResolvers>()
            .AddResolver<OrderResolver>()
            .AddErrorFilter<ValidationErrorFilter>()
            .AddProjections()
            .AddFiltering()
            .AddSorting();

        return services;
    }
}

