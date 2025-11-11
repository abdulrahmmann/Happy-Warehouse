namespace HappyWarehouse.Domain.Bases;

/// <summary>
/// Represents a generic interface for entities with a typed identifier.
/// </summary>
/// <typeparam name="T">The type of the entity identifier.</typeparam>
public interface IEntity<T> : IEntity
{
    /// <summary> The unique identifier of the entity. </summary>
    T Id { get; set; }
}
    
/// <summary>
/// Represents a non-generic base interface for all entities.
/// </summary>
public interface IEntity
{
    /// <summary> The date and time when the entity was created. </summary>
    DateTime? CreatedAt { get; set; }

    /// <summary> The user who created the entity. </summary>
    string? CreatedBy { get; set; }

    /// <summary> The date and time when the entity was last modified. </summary>
    DateTime? ModifiedAt { get; set; }

    /// <summary> The user who last modified the entity. </summary>
    string? ModifiedBy { get; set; }

    /// <summary> The date and time when the entity was deleted (soft delete). </summary>
    DateTime? DeletedAt { get; set; }

    /// <summary> The user who deleted the entity. </summary>
    string? DeletedBy { get; set; }

    /// <summary> The date and time when the entity was restored. </summary>
    DateTime? RestoredAt { get; set; }

    /// <summary> The user who restored the entity. </summary>
    string? RestoredBy { get; set; }
}