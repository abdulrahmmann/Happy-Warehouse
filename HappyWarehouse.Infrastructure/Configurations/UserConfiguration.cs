using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappyWarehouse.Infrastructure.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.HasIndex(u => u.UserName).IsUnique();
        
        builder.Property(u => u.UserName).HasMaxLength(60).IsRequired();
        
        builder.Property(u => u.FullName).HasMaxLength(160).IsRequired();
        
        builder.Property(u => u.Email).HasMaxLength(120).IsRequired();

        builder.Property(u => u.Role) .IsRequired();
        
        builder.Property(u => u.IsActive).HasDefaultValue(true);
        
        builder.Property(u => u.IsDeleted).HasDefaultValue(false);
        
        builder.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(u => u.UpdatedAt).IsRequired(false);
        
        builder.Property(u => u.DeletedAt).IsRequired(false);
    }
}