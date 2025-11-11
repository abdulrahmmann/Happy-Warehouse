using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HappyWarehouse.Application.Features.UsersFeature.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HappyWarehouse.Application.Features.UsersFeature.TokenServices.GeneratePrincipalJwtToken;

public class GeneratePrincipalFromJwtTokenService: IGeneratePrincipalFromJwtTokenService
{
    #region Instance Fields
    private readonly IConfiguration _configuration;
    #endregion

    #region Inject Instances Into Constructor
    public GeneratePrincipalFromJwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    #endregion
    
    public ClaimsPrincipal GetPrincipalFromJwtToken(string? token)
    {
        var jwtSettings = _configuration.GetSection("Jwt").Get<JwtSettings>()!;
        
        var tokenValidation = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            
            ValidAudience = jwtSettings.Audience,
            ValidIssuer = jwtSettings.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SECRET_KEY)),
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var principal = tokenHandler.ValidateToken(token, tokenValidation, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }
}