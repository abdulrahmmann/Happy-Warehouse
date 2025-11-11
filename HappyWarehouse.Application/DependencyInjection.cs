using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GeneratePrincipalJwtToken;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateRefreshToken;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateToken;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HappyWarehouse.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Register JWT Services
        services.AddScoped<IGenerateRefreshTokenService, GenerateRefreshTokenService>();
        services.AddScoped<IGenerateTokenService, GenerateTokenService>();
        services.AddScoped<IGeneratePrincipalFromJwtTokenService, GeneratePrincipalFromJwtTokenService>();
        
        return services;
    }
}