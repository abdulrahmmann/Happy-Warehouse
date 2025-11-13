using System.Net;
using System.Text.Json.Serialization;

namespace HappyWarehouse.Application.Common;

/// <summary>
/// Standardized response wrapper for all NON User API results.
/// </summary>
/// <typeparam name="T">The type of the response data.</typeparam>
public class BaseResponse<T>
{
    [JsonPropertyOrder(1)]
    public int? TotalCount { get; set; }
    
    [JsonPropertyOrder(2)]
    public HttpStatusCode  HttpStatusCode  { get; set; }
    
    [JsonPropertyOrder(3)]
    public string Message { get; set; }
    
    [JsonPropertyOrder(4)]
    public string Errors { get; set; }
    
    [JsonPropertyOrder(5)]
    public DateTime Timestamp { get; set; }
    
    [JsonPropertyOrder(99)] // Ensures `data` appears last
    public T? Data { get; set; }
    
    
    #region Constructors
    public BaseResponse() { }
    private BaseResponse(T? data, HttpStatusCode httpStatusCode, string message, int? totalCount = null)
    {
        TotalCount = totalCount;
        HttpStatusCode = httpStatusCode;
        Message = message;
        Timestamp = DateTime.UtcNow;
        Data = data;
    }

    private BaseResponse(HttpStatusCode httpStatusCode, string message)
        : this(default, httpStatusCode, message) { }
    
    #endregion
    
    
    #region ✅ Success Responses

    /// <summary>200 OK — Operation succeeded and returns data.</summary>
    public static BaseResponse<T> Success(T? data, string message = "Operation successful", int? totalCount = null)
        => new(data, HttpStatusCode.OK, message, totalCount);
    
    public static BaseResponse<T> Success(string message = "Operation successful")
        => new(HttpStatusCode.OK, message);
    
    /// <summary>201 Created — Resource was created successfully.</summary>
    public static BaseResponse<T> Created(T? data, string message = "Resource created successfully", int? totalCount = null)
        => new(data, HttpStatusCode.Created, message, totalCount);

    /// <summary>201 Created — Resource created without returning data.</summary>
    public static BaseResponse<T> Created(string message = "Resource created successfully")
        => new(HttpStatusCode.Created, message);

    /// <summary>204 No Content — Success without returning data.</summary>
    public static BaseResponse<T> NoContent(string message = "No content available")
        => new(default, HttpStatusCode.NoContent, message);

    #endregion
    
    
    #region ❌ Error Responses

    /// <summary>400 Bad Request — Invalid or malformed request.</summary>
    public static BaseResponse<T> BadRequest(string message = "Bad request")
        => new(HttpStatusCode.BadRequest, message);

    /// <summary>400 Bad Request — Invalid request with additional error data.</summary>
    public static BaseResponse<T> BadRequest(T? data, string message = "Bad request")
        => new(data, HttpStatusCode.BadRequest, message);

    /// <summary>404 Not Found — Requested resource does not exist.</summary>
    public static BaseResponse<T> NotFound(string message = "Resource not found")
        => new(HttpStatusCode.NotFound, message);

    /// <summary>500 Internal Server Error — Unexpected server-side error.</summary>
    public static BaseResponse<T> InternalError(string message = "Internal server error")
        => new(HttpStatusCode.InternalServerError, message);

    /// <summary>422 Unprocessable Entity — Validation failed.</summary>
    public static BaseResponse<T> ValidationError(string message = "Validation failed")
        => new(HttpStatusCode.UnprocessableEntity, message);

    /// <summary>409 Conflict — Conflict in the request (e.g., duplicate resource).</summary>
    public static BaseResponse<T> Conflict(string message = "Conflict detected")
        => new(HttpStatusCode.Conflict, message);

    #endregion
}
