using Microsoft.EntityFrameworkCore;
using TradeBlotterApi.Data;
using TradeBlotterApi.Models;

namespace TradeBlotterApi.Repositories;

public class TradeRepository : ITradeRepository
{
    private readonly TradeDbContext _context;

    public TradeRepository(TradeDbContext context)
    {
        _context = context;
    }

    public async Task<Trade> AddTradeAsync(Trade trade)
    {
        _context.Trades.Add(trade);
        await _context.SaveChangesAsync();
        return trade;
    }

    public async Task<IReadOnlyList<Trade>> GetAllTradesAsync()
    {
        return await _context.Trades
            .OrderByDescending(t => t.Timestamp)
            .ThenByDescending(t => t.Id)
            .ToListAsync();
    }
}
