using System.Security.Cryptography;

namespace HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateRefreshToken;

public class GenerateRefreshTokenService: IGenerateRefreshTokenService
{
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        var randomNumberGenerator = RandomNumberGenerator.Create();
        
        randomNumberGenerator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
}