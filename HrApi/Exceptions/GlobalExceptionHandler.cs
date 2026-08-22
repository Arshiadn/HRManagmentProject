using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.Exceptions; 

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Unhandled exception at {Path}. " +
            "TraceId: {TraceId}, CorrelationId: {CorrelationId}",
            httpContext.Request.Path,
            httpContext.TraceIdentifier,
            httpContext.Items["CorrelationId"]
        );

        var status = exception switch
        {
            NotFoundException => 404,
            ConflictException => 409,
            BadRequestException => 400,
            BusinessRuleException => 400,
            _ => 500
        };

        var title = status switch
        {
            404 => "Resource not found",
            409 => "Request conflict",
            400 => "Invalid request",
            _ => "Internal server error"
        };

        var detail = status == 500
        ? "An unexpected error occurred."
        : exception.Message;

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Type = $"https://httpstatuses.com/{status}"
        };
        problem.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = status;

        await httpContext.Response.WriteAsJsonAsync(
            problem,
            cancellationToken
        );

        return true;
    }
}
