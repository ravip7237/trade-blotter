using TradeBlotterApi.Models;

namespace TradeBlotterApi.Repositories;

public interface ITradeRepository
{
    Task<Trade> AddTradeAsync(Trade trade);
    Task<IReadOnlyList<Trade>> GetAllTradesAsync();
}
