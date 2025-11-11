namespace HappyWarehouse.Application.Features.UsersFeature.DTOs;

public record GetUserDetailsDto(string Id, string UserName, string? Email, string? PhoneNumber, string Role,
    string? RefreshToken, DateTime? RefreshTokenExpiration);