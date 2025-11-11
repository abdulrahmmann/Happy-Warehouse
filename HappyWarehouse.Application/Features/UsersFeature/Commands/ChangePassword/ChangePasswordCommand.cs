using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.ChangePassword;

public record ChangePasswordCommand(string Email, ChangePasswordDto PasswordDto) : ICommand<AuthenticationResponse>;