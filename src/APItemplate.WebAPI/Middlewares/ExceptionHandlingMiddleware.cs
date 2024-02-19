using System.Net;
using System.Text.Json;

namespace APItemplate.WebAPI.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    ///<summary>
    ///Constructor with dependency injection
    ///</summary>
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    ///<summary>
    ///Implementing error handling logic
    ///</summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = ex switch
            {
                _ => (int)HttpStatusCode.InternalServerError
            };

            await response.WriteAsync(JsonSerializer.Serialize(new { message = ex?.Message }));
        }
    }
}

///<summary>
///Extensions for ExceptionHandlingMiddleware
///</summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    ///<summary>
    ///Allowing to use "app.UseExceptionHandlingMiddleware();" in Program.cs
    ///</summary>
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
