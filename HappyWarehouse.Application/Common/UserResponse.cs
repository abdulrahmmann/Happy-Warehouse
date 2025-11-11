using System.Net;
using System.Text.Json.Serialization;

namespace HappyWarehouse.Application.Common;

public class UserResponse<T>
{
    [JsonPropertyOrder(1)]
    public int? TotalCount { get; set; }

    [JsonPropertyOrder(2)]
    public HttpStatusCode HttpStatusCode { get; set; }
    
    [JsonPropertyOrder(3)]
    public string? Email { get; set; }

    [JsonPropertyOrder(4)]
    public string Message { get; set; }

    [JsonPropertyOrder(5)]
    public DateTime Timestamp { get; set; }

    [JsonPropertyOrder(99)]
    public T? Data { get; set; }

    public UserResponse(int? totalCount, HttpStatusCode httpStatusCode, string message, T? data)
    {
        TotalCount = totalCount;
        HttpStatusCode = httpStatusCode;
        Message = message;
        Timestamp = DateTime.UtcNow;
        Data = data;
    }

    public UserResponse(string message, HttpStatusCode httpStatusCode)
    {
        Message = message;
        HttpStatusCode = httpStatusCode;
        Timestamp = DateTime.UtcNow;
    }

    public UserResponse(HttpStatusCode httpStatusCode, string? email, string message)
    {
        HttpStatusCode = httpStatusCode;
        Email = email;
        Message = message;
    }

    public static UserResponse<T> Success(int? totalCount, HttpStatusCode httpStatusCode, string message, T? data)
    {
        return new UserResponse<T>(totalCount, httpStatusCode, message, data);
    }
    
    public static UserResponse<T> Created(string? email, string message = "User Created Successfully")
    {
        return new UserResponse<T>(HttpStatusCode.Created, email, message);
    }
    
    public static UserResponse<T> Failure(string message, HttpStatusCode httpStatusCode)
    {
        return new UserResponse<T>(message, httpStatusCode);
    }
}