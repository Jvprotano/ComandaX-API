using ComandaX.Application.Interfaces;
using ComandaX.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ComandaX.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}

