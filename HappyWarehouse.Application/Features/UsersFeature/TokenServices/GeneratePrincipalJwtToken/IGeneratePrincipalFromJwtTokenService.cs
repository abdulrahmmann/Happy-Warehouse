using System.Security.Claims;

namespace HappyWarehouse.Application.Features.UsersFeature.TokenServices.GeneratePrincipalJwtToken;

public interface IGeneratePrincipalFromJwtTokenService
{
    ClaimsPrincipal GetPrincipalFromJwtToken(string? token);
}