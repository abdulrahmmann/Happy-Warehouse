using HappyWarehouse.Domain.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappyWarehouse.Infrastructure.Configurations;

/// <summary>
/// Base configuration for all entities inheriting from Entity
/// </summary>
public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity<int>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // Primary key
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        // DateTime fields as TEXT for SQLite
        builder.Property(e => e.CreatedAt)
            .HasColumnType("TEXT")
            .IsRequired(false);

        builder.Property(e => e.ModifiedAt)
            .HasColumnType("TEXT")
            .IsRequired(false);

        builder.Property(e => e.DeletedAt)
            .HasColumnType("TEXT")
            .IsRequired(false);

        builder.Property(e => e.RestoredAt)
            .HasColumnType("TEXT")
            .IsRequired(false);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.DeletedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.RestoredBy)
            .HasMaxLength(100)
            .IsRequired(false);

        // Soft delete flag
        builder.Property(e => e.IsDeleted)
            .HasColumnType("INTEGER")
            .HasDefaultValue(false);

        // Global query filter
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}