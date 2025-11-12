using FluentValidation;
using HappyWarehouse.Application.Caching;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.CountryFeature.Commands.CreateCountry;
using HappyWarehouse.Application.Features.CountryFeature.Commands.UpdateCountry;
using HappyWarehouse.Application.Features.UsersFeature.Commands.ChangePassword;
using HappyWarehouse.Application.Features.UsersFeature.Commands.DeleteUser;
using HappyWarehouse.Application.Features.UsersFeature.Commands.LoginUser;
using HappyWarehouse.Application.Features.UsersFeature.Commands.RestoreUser;
using HappyWarehouse.Application.Features.UsersFeature.Commands.UpdateUser;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Application.Features.UsersFeature.Queries.GetAllUsers;
using HappyWarehouse.Application.Features.UsersFeature.Queries.GetUserRole;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GeneratePrincipalJwtToken;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateRefreshToken;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateToken;
using HappyWarehouse.Application.Features.UsersFeature.Validations;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.CreateWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.DeleteWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.RestoreWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.UpdateWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Application.Features.WarehouseFeature.Queries.GetWarehouses;
using HappyWarehouse.Application.Features.WarehouseFeature.Queries.GetWarehousesWithItems;
using HappyWarehouse.Application.Features.WarehouseFeature.Validations;
using HappyWarehouse.Application.Features.WarehouseItemFeature.Commands.CreateWarehouseItem;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;
using HappyWarehouse.Application.Features.WarehouseItemFeature.Queries.GetItemByWarehouseId;
using HappyWarehouse.Application.Features.WarehouseItemFeature.Validators;
using HappyWarehouse.Domain.CQRS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HappyWarehouse.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Redis Cache
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        
        // Register JWT Services
        services.AddScoped<IGenerateRefreshTokenService, GenerateRefreshTokenService>();
        services.AddScoped<IGenerateTokenService, GenerateTokenService>();
        services.AddScoped<IGeneratePrincipalFromJwtTokenService, GeneratePrincipalFromJwtTokenService>();
        
        // Register FLUENT VALIDATION
        services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserValidator>();
        
        services.AddValidatorsFromAssemblyContaining<CreateWarehouseValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateWarehouseValidator>();
        
        services.AddValidatorsFromAssemblyContaining<CreateWarehouseItemValidator>();
        
        // Register CQRS Functionality: Request + handler
        // USERS COMMANDS
        services.AddTransient<ICommandHandler<LoginUserCommand, AuthenticationResponse>, LoginUserCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateUserCommand, AuthenticationResponse>, UpdateUserCommandHandler>();
        services.AddTransient<ICommandHandler<SoftDeleteUserCommand, AuthenticationResponse>, SoftDeleteUserCommandHandler>();
        services.AddTransient<ICommandHandler<RestoreUserCommand, AuthenticationResponse>, RestoreUserCommandHandler>();
        services.AddTransient<ICommandHandler<ChangePasswordCommand, AuthenticationResponse>, ChangePasswordCommandHandler>();
        
        // USERS QUERIES
        services.AddTransient<IQueryHandler<GetAllUsersQuery, UserResponse<IEnumerable<UserDetailsDto>>>, GetAllUsersQueryHandler>();
        services.AddTransient<IQueryHandler<GetUserRolesByEmailQuery, string>, GetUserRolesByEmailQueryHandler>();
        
        // COUNTRIES COMMANDS
        services.AddTransient<ICommandHandler<CreateCountryCommand, BaseResponse<string>>, CreateCountryCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateCountryCommand, BaseResponse<string>>, UpdateCountryCommandHandler>();
        
        // WAREHOUSES COMMANDS
        services.AddTransient<ICommandHandler<CreateWarehouseCommand, BaseResponse<string>>, CreateWarehouseCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateWarehouseCommand, BaseResponse<string>>, UpdateWarehouseCommandHandler>();
        services.AddTransient<ICommandHandler<SoftDeleteWarehouseCommand, BaseResponse<string>>, SoftDeleteWarehouseCommandHandler>();
        services.AddTransient<ICommandHandler<RestoreWarehouseCommand, BaseResponse<string>>, RestoreWarehouseCommandHandler>();
        
        // WAREHOUSES QUERIES
        services.AddTransient<IQueryHandler<GetWarehousesQuery, BaseResponse<IEnumerable<WarehouseDto>>>, GetWarehousesQueryHandler>();
        
        // WAREHOUSE ITEMS COMMANDS
        services.AddTransient<ICommandHandler<CreateWarehouseItemCommand, BaseResponse<string>>, CreateWarehouseItemCommandHandler>();
        
        // WAREHOUSE ITEMS QUERIES
        services.AddTransient<IQueryHandler<GetWarehousesWithItemsQuery, BaseResponse<IEnumerable<WarehousesWithItemsDto>>>, GetWarehousesWithItemsQueryHandler>();
        services.AddTransient<IQueryHandler<GetItemsByWarehouseIdQuery, BaseResponse<IEnumerable<WarehouseItemDto>>>, GetItemsByWarehouseIdQueryHandler>();
        
        return services;
    }
}