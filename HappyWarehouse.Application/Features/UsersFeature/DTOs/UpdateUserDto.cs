namespace HappyWarehouse.Application.Features.UsersFeature.DTOs;

public record UpdateUserDto(string? UserName, string? FullName, string? Email, string? PhoneNumber);