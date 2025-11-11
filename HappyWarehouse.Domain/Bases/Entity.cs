namespace HappyWarehouse.Domain.Bases;

/// <summary>
/// Provides a base implementation for all entities in the domain,
/// including audit fields and soft-delete support.
/// </summary>
/// <typeparam name="T">The type of the entity identifier.</typeparam>
public abstract class Entity<T>: IEntity<T>
{
    /// <summary> The unique identifier of the entity. </summary>
    public T Id { get; set; } = default!;

    /// <summary> The date and time when the entity was created. </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary> The user who created the entity. </summary>
    public string? CreatedBy { get; set; }

    /// <summary> The date and time when the entity was last modified. </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary> The user who last modified the entity. </summary>
    public string? ModifiedBy { get; set; }

    /// <summary> The date and time when the entity was deleted (soft delete). </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary> The user who deleted the entity. </summary>
    public string? DeletedBy { get; set; }

    /// <summary> The date and time when the entity was restored. </summary>
    public DateTime? RestoredAt { get; set; }

    /// <summary> The user who restored the entity. </summary>
    public string? RestoredBy { get; set; }

    /// <summary> Indicates whether the entity is soft-deleted. </summary>
    public bool IsDeleted { get; set; } = false;

    // -----------------------------
    // Helper Methods
    // -----------------------------

    /// <summary>
    /// Marks the entity as created and sets audit information.
    /// </summary>
    /// <param name="createdBy">The user who created the entity.</param>
    protected void MarkCreated(string? createdBy = null)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
        IsDeleted = false;
    }

    /// <summary>
    /// Marks the entity as modified and updates audit information.
    /// </summary>
    /// <param name="modifiedBy">The user who modified the entity.</param>
    protected void MarkModified(string? modifiedBy = null)
    {
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    /// <summary>
    /// Marks the entity as soft-deleted and records audit information.
    /// </summary>
    /// <param name="deletedBy">The user who deleted the entity.</param>
    protected void MarkDeleted(string? deletedBy = null)
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }
    }
    
    /// <summary>
    /// Restores a soft-deleted entity and updates audit information.
    /// </summary>
    /// <param name="restoredBy">The user who restored the entity.</param>
    protected void MarkRestored(string? restoredBy = null)
    {
        if (IsDeleted) 
        {
            IsDeleted = false;
            RestoredAt = DateTime.UtcNow;
            RestoredBy = restoredBy;
            DeletedAt = null;
            DeletedBy = null;
        }
    }
}
