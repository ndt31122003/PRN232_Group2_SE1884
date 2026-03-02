using Microsoft.AspNetCore.Diagnostics;

namespace PRN232_EbayClone.Api.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var logId = DateTime.Now.ToString("ddMMyyyyHHmmss");
        _logger.LogError(exception, "[#{logId}] An unhandled exception occurred.", logId);

        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Server Error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = $"[#{logId}] An unhandled exception occurred."
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
