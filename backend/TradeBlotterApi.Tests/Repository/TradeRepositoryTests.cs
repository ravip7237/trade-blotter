using TradeBlotterApi.Data;
using TradeBlotterApi.Enum;
using TradeBlotterApi.Models;
using TradeBlotterApi.Repositories;
using Xunit;

namespace TradeBlotterApi.Tests.Repository;

public class TradeRepositoryTests
{
    [Fact]
    public async Task AddAndGetTrades_ReturnsNewestFirst()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"tradeblotter-tests-{Guid.NewGuid():N}.db");
        var dbContext = new TradeDbContext(tempPath);
        await dbContext.Database.EnsureCreatedAsync();

        var repository = new TradeRepository(dbContext);
        var firstTrade = new Trade
        {
            Symbol = "AAPL",
            Side = TradeSide.Buy,
            Quantity = 10,
            Price = 100.5m,
            Timestamp = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc)
        };
        var secondTrade = new Trade
        {
            Symbol = "MSFT",
            Side = TradeSide.Sell,
            Quantity = 5,
            Price = 200m,
            Timestamp = new DateTime(2024, 1, 1, 11, 0, 0, DateTimeKind.Utc)
        };

        await repository.AddTradeAsync(firstTrade);
        await repository.AddTradeAsync(secondTrade);

        var trades = await repository.GetAllTradesAsync();

        Assert.Collection(
            trades,
            trade => Assert.Equal("MSFT", trade.Symbol),
            trade => Assert.Equal("AAPL", trade.Symbol));
    }

    [Fact]
    public async Task GetAllTradesAsync_ReturnsEmptyList_WhenNoTradesExist()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"tradeblotter-tests-{Guid.NewGuid():N}.db");
        var dbContext = new TradeDbContext(tempPath);
        await dbContext.Database.EnsureCreatedAsync();

        var repository = new TradeRepository(dbContext);

        var trades = await repository.GetAllTradesAsync();

        Assert.Empty(trades);
    }

    [Fact]
    public async Task AddTradeAsync_PersistsTradeWithoutRepositoryValidation()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"tradeblotter-tests-{Guid.NewGuid():N}.db");
        var dbContext = new TradeDbContext(tempPath);
        await dbContext.Database.EnsureCreatedAsync();

        var repository = new TradeRepository(dbContext);
        var trade = new Trade
        {
            Symbol = "   ",
            Side = TradeSide.Buy,
            Quantity = 0,
            Price = -10m,
            Timestamp = DateTime.UtcNow
        };

        var createdTrade = await repository.AddTradeAsync(trade);

        Assert.Equal(trade.Symbol, createdTrade.Symbol);
        Assert.Equal(0, createdTrade.Quantity);
    }
}
