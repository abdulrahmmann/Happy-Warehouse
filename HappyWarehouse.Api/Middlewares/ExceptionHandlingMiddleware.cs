namespace HappyWarehouse.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            logger.LogInformation("Middleware executed before request");

            await next(context);

            logger.LogInformation("Middleware executed after request");
        }
        catch (Exception e)
        {
            logger.LogError($"{e.GetType().ToString()}: {e.Message}");

            if (e.InnerException is not null)
            {
                logger.LogError($"{e.InnerException.GetType().ToString()}: {e.InnerException.Message}");
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new { Message = e.Message, Type = e.GetType().ToString() });
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}