using TradeBlotterApi.Dtos;
using TradeBlotterApi.Mapper;
using TradeBlotterApi.Models;
using TradeBlotterApi.Services;

namespace TradeBlotterApi.Tests.Services;

public class StubTradeService : ITradeService
{
    private readonly List<TradeResponse> _trades;

    public StubTradeService(params Trade[] trades)
    {
        _trades = trades.Select(TradeMapper.ToResponse).ToList();
    }

    public Task<TradeResponse> AddTradeAsync(TradeRequest request)
    {
        var trade = new TradeResponse
        {
            Symbol = request.Symbol.Trim().ToUpperInvariant(),
            Side = request.Side.Trim().ToUpperInvariant(),
            Quantity = request.Quantity,
            Price = request.Price,
            Timestamp = request.Timestamp == default ? DateTime.UtcNow : request.Timestamp
        };

        _trades.Add(trade);
        return Task.FromResult(trade);
    }

    public Task<IReadOnlyList<TradeResponse>> GetAllTradesAsync() => Task.FromResult<IReadOnlyList<TradeResponse>>(_trades);
}
