using System.Net;
using FluentValidation;

namespace CodeBaseCQRSBasic.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error occurred.");
            await WriteErrorResponseAsync(context, HttpStatusCode.BadRequest, ex.Message, "One or more validations failed.");
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Requested resource was not found.");
            await WriteErrorResponseAsync(context, HttpStatusCode.NotFound, ex.Message, "The requested resource does not exist.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred.");
            await WriteErrorResponseAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.", ex.Message);
        }
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, HttpStatusCode statusCode, string message, string details)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsJsonAsync(new
        {
            message,
            details,
            statusCode = (int)statusCode
        });
    }
}
