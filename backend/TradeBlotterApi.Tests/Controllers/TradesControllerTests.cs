using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TradeBlotterApi.Controllers;
using TradeBlotterApi.Dtos;
using TradeBlotterApi.Enum;
using TradeBlotterApi.Models;
using TradeBlotterApi.Services;
using TradeBlotterApi.Tests.Services;
using TradeBlotterApi.Validators;
using Xunit;

namespace TradeBlotterApi.Tests.Controllers;

public class TradesControllerTests
{
    [Fact]
    public async Task PostTrade_ReturnsBadRequest_ForInvalidPayload()
    {
        var tradeService = new StubTradeService();
        var positionService = new PositionService(new StubTradeRepository());
        var validator = new TradeRequestValidator();
        var controller = new TradesController(tradeService, positionService, validator);

        var result = await controller.CreateTrade(new TradeRequest { Symbol = "", Side = "Buy", Quantity = 1, Price = 10m, Timestamp = DateTime.UtcNow });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

        var details = Assert.IsType<ValidationProblemDetails>(badRequest.Value);
        Assert.Contains("Symbol", details.Errors.Keys);
        Assert.Contains("Symbol is required.", details.Errors["Symbol"]);
    }

    [Fact]
    public async Task GetTrade_ReturnsTrade_WhenFound()
    {
        var tradeService = new StubTradeService(new Trade { Id = 1, Symbol = "AAPL", Side = TradeSide.Buy, Quantity = 5, Price = 100m, Timestamp = DateTime.UtcNow });
        var positionService = new PositionService(new StubTradeRepository());
        var validator = new TradeRequestValidator();
        var controller = new TradesController(tradeService, positionService, validator);

        var result = await controller.GetAllTrades();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var trades = Assert.IsAssignableFrom<IReadOnlyList<TradeResponse>>(okResult.Value);
        Assert.Single(trades);
        Assert.Equal("AAPL", trades[0].Symbol);
    }

    [Fact]
    public async Task GetTrades_ReturnsTradesFromService()
    {
        var tradeService = new StubTradeService(new Trade { Id = 1, Symbol = "AAPL", Side = TradeSide.Buy, Quantity = 5, Price = 100m, Timestamp = DateTime.UtcNow });
        var positionService = new PositionService(new StubTradeRepository());
        var validator = new TradeRequestValidator();
        var controller = new TradesController(tradeService, positionService, validator);

        var result = await controller.GetAllTrades();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var trades = Assert.IsAssignableFrom<IReadOnlyList<TradeResponse>>(okResult.Value);
        Assert.Single(trades);
        Assert.Equal("AAPL", trades[0].Symbol);
    }
}
