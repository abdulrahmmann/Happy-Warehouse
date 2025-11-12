using HappyWarehouse.Domain.IdentityEntities;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.Context;
using HappyWarehouse.Infrastructure.Repository;
using HappyWarehouse.Infrastructure.UOF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HappyWarehouse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // REGISTER DB CONTEXT Sqlite
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });
        
        // REGISTER IDENTITY
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 1;
       
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()

            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, int>>()

            .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, int>>();
        
        
        // REGISTER UNIT OF WORK
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // REGISTER GENERIC REPOSITORY  
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        
        // REGISTER REPOSITORIES    
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        
        return services;
    }
}
