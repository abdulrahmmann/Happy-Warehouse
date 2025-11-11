namespace HappyWarehouse.Application.Features.UsersFeature.DTOs;

public record UserDetailsDto(string Id, string? UserName, string? FullName, string? Email, string? PhoneNumber, 
    string Role, bool IsDeleted, bool IsActive, 
    DateTime CreatedAt, DateTime? UpdatedAt, DateTime? DeletedAt);
