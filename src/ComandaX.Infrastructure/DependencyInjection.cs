using Microsoft.Extensions.DependencyInjection;
using ComandaX.Application.Interfaces;
using ComandaX.Infrastructure.Persistence;
using ComandaX.Infrastructure.Persistence.Repository;
using ComandaX.Infrastructure.Services;

namespace ComandaX.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register individual repositories (for backward compatibility and direct injection if needed)
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICustomerTabRepository, CustomerTabRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

        // Register AbacatePay service with HttpClient
        services.AddHttpClient<IAbacatePayService, AbacatePayService>();

        // Register Subscription Notification Service
        services.AddScoped<ISubscriptionNotificationService, SubscriptionNotificationService>();

        return services;
    }
}

