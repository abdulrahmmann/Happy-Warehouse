using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.DeleteUser;

public record SoftDeleteUserCommand(string Email) : ICommand<AuthenticationResponse>;
