using Microsoft.AspNetCore.Identity;

namespace HappyWarehouse.Domain.IdentityEntities;

/// <summary>
/// Represents an application user with basic authentication and Info.
/// </summary>
public class ApplicationUser: IdentityUser<int>
{
    /// <summary>
    /// The fullName chosen by the user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    public string Role { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    
    
    // Refresh Token fields
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiration { get; set; }

    public void MarkUserAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
    
    public void MarkUserAsModified()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}