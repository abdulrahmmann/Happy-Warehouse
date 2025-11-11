using HappyWarehouse.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HappyWarehouse.Domain.IdentityEntities;

/// <summary>
/// Represents an application user with authentication and account management details.
/// </summary>
public class ApplicationUser: IdentityUser<int>
{
    /// <summary>
    /// The fullName chosen by the user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Represent the user account status : active or disabled
    /// If <see langword="false"/>, the user cannot log in.
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Represent the role assigned to the user.
    /// Possible values include:
    /// <list type="bullet">
    /// <item>Admin</item>
    /// <item>User</item>
    /// <item>Management</item>
    /// <item>Auditor</item>
    /// </list>
    /// </summary>
    public string Role { get; set; } = string.Empty;
    
    /// <summary>
    /// Represent the date and time when the user account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Represent the date and time when the user account was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Represent the date and time when the user account was soft-deleted.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
    
    /// <summary>
    /// Represent the date and time when the user account was restored.
    /// </summary>
    public DateTime? RestoredAt { get; set; }
    
    /// <summary>
    /// Represent the value indicating whether the user account is softly-deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
    
    
    #region Refresh Token fields
    /// <summary>
    /// Represent the refresh token assigned to the user.
    /// </summary>
    public string? RefreshToken { get; set; }
    
    /// <summary>
    /// Represent the expiration date and time of the refresh token.
    /// </summary>
    public DateTime? RefreshTokenExpiration { get; set; }
    #endregion
    
    /// <summary> Navigation Property: One user can create many warehouses </summary>
    public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();

    /// <summary> Navigation Property: One user can create many warehouse items. </summary>
    public ICollection<WarehouseItem> CreatedItems { get; set; } = new List<WarehouseItem>();

    
    #region Helper Methods
    /// <summary>
    ///  Marks the user account as soft-deleted and sets <see cref="DeletedAt"/> to the current UTC time.
    /// </summary>
    public void MarkUserAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Updates the <see cref="UpdatedAt"/> property to the current UTC time, indicating the account was modified.
    /// </summary>
    public void MarkUserAsModified()
    {
        UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Disables the user account by setting <see cref="IsActive"/> to <see langword="false"/>.
    /// </summary>
    public void MarkUserAsDisabled()
    {
        IsActive = false;
    }
    
    /// <summary>
    /// Restores the user account by setting <see cref="IsDeleted"/> to <see langword="false"/>,
    /// <see cref="IsActive"/> to <see langword="true"/>, and updating <see cref="RestoredAt"/> to the current UTC time.
    /// </summary>
    public void MarkUserAsRestored()
    {
        IsDeleted = false;
        IsActive = true;
        RestoredAt = DateTime.UtcNow;
    }
    #endregion
    
    
}