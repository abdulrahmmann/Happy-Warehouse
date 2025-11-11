using HappyWarehouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappyWarehouse.Infrastructure.Configurations;

/// <summary>
/// Configuration for Warehouse Item Entity.
/// </summary>
public class WarehouseItemConfiguration: BaseEntityConfiguration<WarehouseItem>
{
    public override void Configure(EntityTypeBuilder<WarehouseItem> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("WarehouseItems");
        
        builder.HasKey(temp => temp.Id).HasName("PK_WarehouseItemId");

        builder.HasIndex(temp => temp.ItemName).IsUnique();
        
        builder.Property(x => x.ItemName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.SkuCode)
            .HasMaxLength(100);

        builder.Property(x => x.Qty)
            .IsRequired();

        builder.Property(x => x.CostPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.MsrpPrice)
            .HasColumnType("decimal(18,2)");

        // Relationship
        builder.HasOne(x => x.Warehouse)
            .WithMany(w => w.WarehouseItems)
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}