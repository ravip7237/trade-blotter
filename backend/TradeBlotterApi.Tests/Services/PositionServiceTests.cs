using TradeBlotterApi.Dtos;
using TradeBlotterApi.Enum;
using TradeBlotterApi.ExceptionHandling;
using TradeBlotterApi.Models;
using TradeBlotterApi.Repositories;
using TradeBlotterApi.Services;
using Xunit;

namespace TradeBlotterApi.Tests.Services;

public class PositionServiceTests
{
    private readonly PositionService _service = new(new StubTradeRepository());

    [Fact]
    public void CalculatePositions_ReturnsSinglePosition_ForBuyTradeSequence()
    {
        var trades = new List<Trade>
        {
            new()
            {
                Symbol = "AAPL",
                Side = TradeSide.Buy,
                Quantity = 10,
                Price = 100m,
                Timestamp = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            }
        };

        var positions = _service.CalculatePositions(trades);

        var position = Assert.Single(positions);
        Assert.Equal("AAPL", position.Symbol);
        Assert.Equal(10, position.NetQuantity);
        Assert.Equal(100m, position.AverageCost);
    }

    [Fact]
    public void CalculatePositions_UsesAverageCost_AfterMixedBuysAndSells()
    {
        var trades = new List<Trade>
        {
            new()
            {
                Symbol = "AAPL",
                Side = TradeSide.Buy,
                Quantity = 10,
                Price = 100m,
                Timestamp = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Symbol = "AAPL",
                Side = TradeSide.Buy,
                Quantity = 10,
                Price = 110m,
                Timestamp = new DateTime(2024, 1, 1, 11, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Symbol = "AAPL",
                Side = TradeSide.Sell,
                Quantity = 5,
                Price = 115m,
                Timestamp = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
            }
        };

        var positions = _service.CalculatePositions(trades);

        var position = Assert.Single(positions);
        Assert.Equal("AAPL", position.Symbol);
        Assert.Equal(15, position.NetQuantity);
        Assert.Equal(105m, position.AverageCost);
    }

    [Fact]
    public void CalculatePositions_OmitsZeroNetPositions()
    {
        var trades = new List<Trade>
        {
            new()
            {
                Symbol = "AAPL",
                Side = TradeSide.Buy,
                Quantity = 10,
                Price = 100m,
                Timestamp = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Symbol = "AAPL",
                Side = TradeSide.Sell,
                Quantity = 10,
                Price = 105m,
                Timestamp = new DateTime(2024, 1, 1, 11, 0, 0, DateTimeKind.Utc)
            }
        };

        var positions = _service.CalculatePositions(trades);

        Assert.Empty(positions);
    }

    [Fact]
    public void CalculatePositions_Throws_ForNegativeQuantityTrade()
    {
        var trades = new List<Trade>
        {
            new()
            {
                Symbol = "AAPL",
                Side = TradeSide.Buy,
                Quantity = -5,
                Price = 100m,
                Timestamp = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            }
        };

        Assert.Throws<TradeValidationException>(() => _service.CalculatePositions(trades));
    }

    [Fact]
    public void CalculatePositions_IgnoresSell_WhenSellWouldDrivePositionNegative()
    {
        var trades = new List<Trade>
        {
            new()
            {
                Symbol = "AAPL",
                Side = TradeSide.Buy,
                Quantity = 5,
                Price = 100m,
                Timestamp = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Symbol = "AAPL",
                Side = TradeSide.Sell,
                Quantity = 10,
                Price = 105m,
                Timestamp = new DateTime(2024, 1, 1, 11, 0, 0, DateTimeKind.Utc)
            }
        };

        var positions = _service.CalculatePositions(trades);

        var position = Assert.Single(positions);
        Assert.Equal("AAPL", position.Symbol);
        Assert.Equal(5, position.NetQuantity);
        Assert.Equal(100m, position.AverageCost);
    }

    [Fact]
    public void CalculatePositions_UsesChronologicalOrder_WhenInputIsNewestFirst()
    {
        var trades = new List<Trade>
        {
            new()
            {
                Id = 2,
                Symbol = "NVDA",
                Side = TradeSide.Sell,
                Quantity = 13,
                Price = 101m,
                Timestamp = new DateTime(2024, 1, 1, 11, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Id = 1,
                Symbol = "NVDA",
                Side = TradeSide.Buy,
                Quantity = 34,
                Price = 100m,
                Timestamp = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            }
        };

        var positions = _service.CalculatePositions(trades);

        var position = Assert.Single(positions);
        Assert.Equal("NVDA", position.Symbol);
        Assert.Equal(21, position.NetQuantity);
        Assert.Equal(100m, position.AverageCost);
    }
}
