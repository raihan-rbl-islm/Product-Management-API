using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ProductManagementApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private static readonly JsonSerializerOptions _jsonOptions = new() 
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "An exception occurred while processing the request.");

        var statusCode = ex switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            ArgumentException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        var title = ex switch
        {
            KeyNotFoundException => "Not Found",
            ArgumentException => "Bad Request",
            _ => "Internal Server Error"
        };

        var detail = ex switch
        {
            KeyNotFoundException or ArgumentException => ex.Message,
            _ => "An unexpected error occurred. Please try again later."
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        var json = JsonSerializer.Serialize(problemDetails, _jsonOptions);

        await context.Response.WriteAsync(json);
    }
}