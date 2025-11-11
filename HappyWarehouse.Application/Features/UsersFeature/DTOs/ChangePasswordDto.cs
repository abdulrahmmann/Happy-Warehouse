namespace HappyWarehouse.Application.Features.UsersFeature.DTOs;

public record ChangePasswordDto(string CurrentPassword, string NewPassword);