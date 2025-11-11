using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.LoginUser;

public record LoginUserCommand(LoginUserDto UserDto): ICommand<AuthenticationResponse>;