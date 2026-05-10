using Kuva.Auth.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kuva.Auth.Service.Middlewares;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AuthException ex)
        {
            await WriteProblemAsync(context, ex.StatusCode, ex.Code, ex.Message, ex.Details);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception while processing request {Path}", context.Request.Path);
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "unexpected_error", "Erro inesperado.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string code, string message, IReadOnlyCollection<string>? details = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = message,
            Type = code
        };
        if (details is not null)
        {
            problem.Extensions["details"] = details;
        }

        await context.Response.WriteAsJsonAsync(problem);
    }
}
