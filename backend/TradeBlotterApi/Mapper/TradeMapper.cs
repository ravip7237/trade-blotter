using TradeBlotterApi.Dtos;
using TradeBlotterApi.Enum;
using TradeBlotterApi.Models;

namespace TradeBlotterApi.Mapper;

public static class TradeMapper
{
    public static Trade ToModel(TradeRequest request) => new()
    {
        Symbol = request.Symbol.Trim().ToUpperInvariant(),
        Side = System.Enum.Parse<TradeSide>(request.Side, true),
        Quantity = request.Quantity,
        Price = request.Price,
        Timestamp = request.Timestamp == default ? DateTime.UtcNow : request.Timestamp
    };

    public static TradeResponse ToResponse(Trade trade) => new()
    {
        Id = trade.Id,
        Symbol = trade.Symbol,
        Side = trade.Side.ToString(),
        Quantity = trade.Quantity,
        Price = trade.Price,
        Timestamp = trade.Timestamp
    };
}
