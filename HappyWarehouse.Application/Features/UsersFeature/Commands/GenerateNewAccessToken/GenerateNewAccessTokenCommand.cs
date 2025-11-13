using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.Models;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.GenerateNewAccessToken;

public record GenerateNewAccessTokenCommand(TokenModel TokenModel): ICommand<AuthenticationResponse>;