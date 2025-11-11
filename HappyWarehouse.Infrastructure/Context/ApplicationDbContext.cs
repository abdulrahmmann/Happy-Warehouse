using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.Infrastructure.Context;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    protected ApplicationDbContext() { }
    
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        builder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsDeleted);
    }
}