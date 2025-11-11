using HappyWarehouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappyWarehouse.Infrastructure.Configurations;

/// <summary>
/// Configuration for Country Entity.
/// </summary>
public class CountryConfiguration: BaseEntityConfiguration<Country>
{
    public override void Configure(EntityTypeBuilder<Country> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("Countries");
        
        builder.HasKey(temp => temp.Id).HasName("PK_CountryId");

        builder.HasIndex(temp => temp.Name);
        
        builder.Property(temp => temp.Name).IsRequired().HasColumnName("CountryName").HasMaxLength(60);
    }
}