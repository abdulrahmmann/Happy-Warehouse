using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.IdentityEntities;

namespace HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateToken;

public interface IGenerateTokenService
{
    AuthenticationResponse GenerateToken(ApplicationUser user);
}