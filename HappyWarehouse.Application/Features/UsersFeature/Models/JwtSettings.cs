namespace HappyWarehouse.Application.Features.UsersFeature.Models;

public class JwtSettings
{
    public string Audience  { get; set; }
    public string Issuer  { get; set; }
    public string SECRET_KEY  { get; set; }
    public string EXPIRATION_MINUTES  { get; set; }
}