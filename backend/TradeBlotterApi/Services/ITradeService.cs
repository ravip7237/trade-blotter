using TradeBlotterApi.Dtos;

namespace TradeBlotterApi.Services;

public interface ITradeService
{
    Task<TradeResponse> AddTradeAsync(TradeRequest request);
    Task<IReadOnlyList<TradeResponse>> GetAllTradesAsync();
}
