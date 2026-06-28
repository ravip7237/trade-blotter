using System.Text.Json;

using FluentValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

using TradeBlotterApi.ExceptionHandling;

using Xunit;

namespace TradeBlotterApi.Tests.ExceptionHandling;

public class GlobalExceptionHandlerTests
{
    private readonly GlobalExceptionHandler _handler = new(NullLogger<GlobalExceptionHandler>.Instance);

    [Fact]
    public async Task TryHandleAsync_ReturnsBadRequest_ForTradeValidationException()
    {
        var context = CreateHttpContext();

        var handled = await _handler.TryHandleAsync(
            context,
            new TradeValidationException("Trade quantity must be greater than zero."),
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);

        var details = await ReadProblemDetailsAsync(context);
        Assert.Equal("Request validation failed.", details.Title);
        Assert.Equal("Trade quantity must be greater than zero.", details.Detail);
    }

    [Fact]
    public async Task TryHandleAsync_ReturnsConflict_ForPositionValidationException()
    {
        var context = CreateHttpContext();

        var handled = await _handler.TryHandleAsync(
            context,
            new PositionValidationException("Sell quantity cannot exceed the current position."),
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);

        var details = await ReadProblemDetailsAsync(context);
        Assert.Equal("Request could not be completed.", details.Title);
        Assert.Equal("Sell quantity cannot exceed the current position.", details.Detail);
    }

    [Fact]
    public async Task TryHandleAsync_ReturnsBadRequest_ForArgumentException()
    {
        var context = CreateHttpContext();

        var handled = await _handler.TryHandleAsync(
            context,
            new ArgumentException("A required argument is missing."),
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);

        var details = await ReadProblemDetailsAsync(context);
        Assert.Equal("Invalid request.", details.Title);
        Assert.Equal("A required argument is missing.", details.Detail);
    }

    [Fact]
    public async Task TryHandleAsync_ReturnsConflict_ForInvalidOperationException()
    {
        var context = CreateHttpContext();

        var handled = await _handler.TryHandleAsync(
            context,
            new InvalidOperationException("The trade cannot be processed in the current state."),
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);

        var details = await ReadProblemDetailsAsync(context);
        Assert.Equal("Request could not be completed.", details.Title);
        Assert.Equal("The trade cannot be processed in the current state.", details.Detail);
    }

    [Fact]
    public async Task TryHandleAsync_ReturnsInternalServerError_ForUnhandledException()
    {
        var context = CreateHttpContext();

        var handled = await _handler.TryHandleAsync(
            context,
            new Exception("database offline"),
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);

        var details = await ReadProblemDetailsAsync(context);
        Assert.Equal("Unexpected server error.", details.Title);
        Assert.Equal("An unexpected error occurred.", details.Detail);
    }

    [Fact]
    public async Task TryHandleAsync_ReturnsBadRequest_ForFluentValidationException()
    {
        var context = CreateHttpContext();
        var exception = new ValidationException("Trade request is invalid.");

        var handled = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);

        var details = await ReadProblemDetailsAsync(context);
        Assert.Equal("Request validation failed.", details.Title);
        Assert.Equal("Trade request is invalid.", details.Detail);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Request.Path = "/positions";
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<ProblemDetails> ReadProblemDetailsAsync(DefaultHttpContext context)
    {
        context.Response.Body.Position = 0;

        var details = await JsonSerializer.DeserializeAsync<ProblemDetails>(context.Response.Body, cancellationToken: CancellationToken.None);

        return Assert.IsType<ProblemDetails>(details);
    }
}