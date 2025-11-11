using System.Net;
using System.Text.Json.Serialization;

namespace HappyWarehouse.Application.Common;
public class AuthenticationResponse
{
    [JsonPropertyOrder(1)]
    public HttpStatusCode HttpStatusCode { get; set; }

    [JsonPropertyOrder(2)]
    public DateTime Timestamp { get; set; }

    [JsonPropertyOrder(3)]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyOrder(4)]
    public List<string>? Errors { get; set; }

    [JsonPropertyOrder(5)]
    public string? Username { get; set; }

    [JsonPropertyOrder(6)]
    public string? Email { get; set; }

    [JsonPropertyOrder(7)]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyOrder(8)] 
    public string? RefreshToken { get; set; } = string.Empty;

    [JsonPropertyOrder(9)]
    public DateTime RefreshTokenExpiration { get; set; } 
    
    [JsonPropertyOrder(20)]
    public DateTime Expiration { get; set; }
    
    // Factory Method For Success Response
    public static AuthenticationResponse Success(string username, string email, string token, string refreshToken, 
        DateTime refreshTokenExpiration, DateTime expiration, string message = "Operation successful")
    {
        return new AuthenticationResponse
        {
            Username = username,
            Email = email,
            Token = token,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = refreshTokenExpiration,
            Expiration = expiration,
            HttpStatusCode = HttpStatusCode.OK,
            Timestamp = DateTime.UtcNow,
            Message = message
        };
    }
    
    public static AuthenticationResponse Success(string message, IEnumerable<string>? errors = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new AuthenticationResponse
        {
            Message = message,
            Errors = errors?.ToList(),
            HttpStatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };
    }

    // Factory Method For Failure Response
    public static AuthenticationResponse Failure(string message, IEnumerable<string>? errors = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new AuthenticationResponse
        {
            Message = message,
            Errors = errors?.ToList(),
            HttpStatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };
    }
}