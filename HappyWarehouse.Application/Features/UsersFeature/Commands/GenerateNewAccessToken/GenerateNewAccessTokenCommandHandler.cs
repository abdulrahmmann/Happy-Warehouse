using System.Security.Claims;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GeneratePrincipalJwtToken;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateToken;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.GenerateNewAccessToken;

public class GenerateNewAccessTokenCommandHandler(
    IGeneratePrincipalFromJwtTokenService principalService, 
    UserManager<ApplicationUser> userManager, 
    IGenerateTokenService tokenService
    ) : ICommandHandler<GenerateNewAccessTokenCommand, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> HandleAsync(GenerateNewAccessTokenCommand command, CancellationToken cancellationToken = default)
    {
        if (command.TokenModel is null)
        {
            return AuthenticationResponse.Failure("TokenModel is null");
        }

        var principal = principalService.GetPrincipalFromJwtToken(command.TokenModel.Token);
            
        if (principal is null) return AuthenticationResponse.Failure("Principal is null");

        var email = principal.FindFirstValue(ClaimTypes.Email)!;

        var user = await userManager.FindByEmailAsync(email);

        if (user == null || user.RefreshToken != command.TokenModel.RefreshToken || user.RefreshTokenExpiration <= DateTime.UtcNow)
        {
            return AuthenticationResponse.Failure("Principal is null");
        }

        var response = tokenService.GenerateToken(user);

        user.RefreshToken = response.RefreshToken;
        user.RefreshTokenExpiration = response.RefreshTokenExpiration;

        await userManager.UpdateAsync(user);

        return response;
    }
}