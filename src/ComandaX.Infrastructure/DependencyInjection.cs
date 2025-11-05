using Microsoft.Extensions.DependencyInjection;
using ComandaX.Application.Interfaces;
using ComandaX.Infrastructure.Persistence.Repository;

namespace ComandaX.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICustomerTabRepository, CustomerTabRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

        return services;
    }
}

