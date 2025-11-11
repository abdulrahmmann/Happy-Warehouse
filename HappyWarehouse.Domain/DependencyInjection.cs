using HappyWarehouse.Domain.CQRS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HappyWarehouse.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // REGISTER DISPATCHER
        services.AddSingleton<Dispatcher>();

        return services;
    }
}
