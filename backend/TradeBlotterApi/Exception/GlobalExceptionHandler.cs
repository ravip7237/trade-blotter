using FluentValidation;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TradeBlotterApi.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title, detail, logLevel) = MapException(exception);

        _logger.Log(logLevel, exception, "Request failed with status code {StatusCode}.", statusCode);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path,
            },
            cancellationToken);

        return true;
    }

    private static (int StatusCode, string Title, string Detail, LogLevel LogLevel) MapException(Exception exception) => exception switch
    {
        ValidationException validationException =>
            (StatusCodes.Status400BadRequest, "Request validation failed.", validationException.Message, LogLevel.Warning),
        TradeValidationException tradeValidationException =>
            (StatusCodes.Status400BadRequest, "Request validation failed.", tradeValidationException.Message, LogLevel.Warning),
        PositionValidationException positionValidationException =>
            (StatusCodes.Status409Conflict, "Request could not be completed.", positionValidationException.Message, LogLevel.Warning),
        ArgumentException argumentException =>
            (StatusCodes.Status400BadRequest, "Invalid request.", argumentException.Message, LogLevel.Warning),
        InvalidOperationException invalidOperationException =>
            (StatusCodes.Status409Conflict, "Request could not be completed.", invalidOperationException.Message, LogLevel.Warning),
        _ =>
            (StatusCodes.Status500InternalServerError, "Unexpected server error.", "An unexpected error occurred.", LogLevel.Error),
    };
}