using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.UpdateUser;

public record UpdateUserCommand(string Email, UpdateUserDto? UserDto): ICommand<AuthenticationResponse>;