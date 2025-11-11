using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.RestoreUser;

public record RestoreUserCommand(string Email): ICommand<AuthenticationResponse>;