using Microsoft.EntityFrameworkCore;
using TradeBlotterApi.Data;
using TradeBlotterApi.Enum;
using TradeBlotterApi.Models;

namespace TradeBlotterApi.Tests.Data;

public static class SeedData
{
    public static async Task InitializeAsync(TradeDbContext context)
    {
        if (await context.Trades.AnyAsync())
        {
            return;
        }

        var seedTrades = new[]
        {
            new Trade
            {
                Symbol = "AAPL",
                Side = TradeSide.Buy,
                Quantity = 10,
                Price = 180.25m,
                Timestamp = new DateTime(2024, 1, 1, 9, 0, 0, DateTimeKind.Utc)
            },
            new Trade
            {
                Symbol = "AAPL",
                Side = TradeSide.Buy,
                Quantity = 5,
                Price = 190.50m,
                Timestamp = new DateTime(2024, 1, 2, 10, 0, 0, DateTimeKind.Utc)
            },
            new Trade
            {
                Symbol = "MSFT",
                Side = TradeSide.Sell,
                Quantity = 8,
                Price = 410.00m,
                Timestamp = new DateTime(2024, 1, 3, 11, 0, 0, DateTimeKind.Utc)
            }
        };

        context.Trades.AddRange(seedTrades);
        await context.SaveChangesAsync();
    }
}
