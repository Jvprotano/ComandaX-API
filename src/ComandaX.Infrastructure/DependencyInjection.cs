using Microsoft.Extensions.DependencyInjection;
using ComandaX.Application.Interfaces;
using ComandaX.Infrastructure.Persistence.Repository;
using ComandaX.Infrastructure.Repository;

namespace ComandaX.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}

