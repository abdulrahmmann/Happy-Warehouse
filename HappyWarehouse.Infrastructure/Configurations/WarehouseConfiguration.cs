using HappyWarehouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappyWarehouse.Infrastructure.Configurations;

/// <summary>
/// Configuration for Warehouse Entity.
/// </summary>
public class WarehouseConfiguration: BaseEntityConfiguration<Warehouse>
{
    public override void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("Warehouses");
        
        builder.HasKey(temp => temp.Id).HasName("PK_WarehouseId");

        builder.HasIndex(temp => temp.Name);
        
        builder.Property(temp => temp.Name).IsRequired().HasColumnName("WarehouseName").HasMaxLength(60);
        
        builder.Property(temp => temp.Address).IsRequired().HasColumnName("AddressName").HasMaxLength(500);
        
        builder.Property(temp => temp.City).IsRequired().HasColumnName("CityName").HasMaxLength(60);
        
        builder.HasOne(wh => wh.Country)
            .WithMany(c => c.Warehouses)
            .HasForeignKey(wh => wh.CountryId)
            .HasConstraintName("FK_Warehouse_Country")
            .OnDelete(DeleteBehavior.NoAction);
    }
}