using TradeBlotterApi.Models;
using TradeBlotterApi.Repositories;

namespace TradeBlotterApi.Tests.Services;

public class StubTradeRepository : ITradeRepository
{
    public Task<Trade> AddTradeAsync(Trade trade) => Task.FromResult(trade);

    public virtual Task<IReadOnlyList<Trade>> GetAllTradesAsync() => Task.FromResult<IReadOnlyList<Trade>>(Array.Empty<Trade>());
}
