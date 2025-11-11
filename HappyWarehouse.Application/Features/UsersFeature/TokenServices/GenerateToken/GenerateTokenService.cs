using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateRefreshToken;
using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateToken;

public class GenerateTokenService: IGenerateTokenService
{
    #region Instance Fields
    private readonly IGenerateRefreshTokenService _refreshTokenService;
    private readonly UserManager<ApplicationUser>  _userManager;
    private readonly IConfiguration  _configuration;
    #endregion

    #region Inject Instances Into Constructor
    public GenerateTokenService(IGenerateRefreshTokenService refreshTokenService, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _refreshTokenService = refreshTokenService;
        _userManager = userManager;
        _configuration = configuration;
    }
    #endregion

    #region Generate Token Method.
    public AuthenticationResponse GenerateToken(ApplicationUser user)
    {
        // Token Expiration Minutes.
        var expirationToken = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));
        
        // Get user role.
        var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
        
        // Get Secret Key.
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SECRET_KEY"]!));
        
        // Create Signing Credentials.
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        
        // Create User Claims.
        var claims = new Claim[]
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email!),
            new (JwtRegisteredClaimNames.Name, user.FullName),
            new (JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new (ClaimTypes.Role, userRole ?? "User")
        };
        
        // Generate Token.
        var tokenGenerator = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expirationToken,
            signingCredentials: signingCredentials
            );
        
        // Write Token.
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var token = tokenHandler.WriteToken(tokenGenerator);
        
        // Return AuthenticationResponse.
        return AuthenticationResponse.Success(
            username: user.UserName!,
            email: user.Email!,
            token: token,
            refreshToken: _refreshTokenService.GenerateRefreshToken(),
            refreshTokenExpiration: DateTime.Now.AddMinutes(Convert.ToInt64(_configuration["RefreshToken:EXPIRATION_MINUTES"])),
            expiration: expirationToken,
            message: "Token Generated Successfully"
            );
    }
    #endregion
    
}