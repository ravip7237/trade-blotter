using TradeBlotterApi.Dtos;
using TradeBlotterApi.Enum;
using TradeBlotterApi.ExceptionHandling;
using TradeBlotterApi.Mapper;
using TradeBlotterApi.Models;
using TradeBlotterApi.Repositories;

namespace TradeBlotterApi.Services;

public class PositionService : IPositionService
{
    private readonly ITradeRepository _tradeRepository;

    public PositionService(ITradeRepository tradeRepository)
    {
        _tradeRepository = tradeRepository;
    }
    public async Task<IReadOnlyList<PositionResponse>> GetPositionsAsync()
    {
        var trades = await _tradeRepository.GetAllTradesAsync();
        return CalculatePositions(trades);
    }

    public IReadOnlyList<PositionResponse> CalculatePositions(IEnumerable<Trade> trades)
    {
        var tradeList = (trades ?? Enumerable.Empty<Trade>()).ToList();

        foreach (var trade in tradeList)
        {
            ValidateTrade(trade);
        }

        var orderedTrades = tradeList
            .OrderBy(trade => trade.Timestamp)
            .ThenBy(trade => trade.Id)
            .ToList();

        var positions = new Dictionary<string, Position>();

        foreach (var trade in orderedTrades)
        {
            if (!positions.TryGetValue(trade.Symbol, out var position))
            {
                position = new Position { Symbol = trade.Symbol };
                positions[trade.Symbol] = position;
            }

            if (trade.Side == TradeSide.Buy)
            {
                var priorNetQuantity = position.NetQuantity;
                position.NetQuantity += trade.Quantity;
                position.AverageCost = priorNetQuantity == 0
                    ? trade.Price
                    : ((position.AverageCost * priorNetQuantity) + (trade.Price * trade.Quantity)) / position.NetQuantity;
            }
            else
            {
                if (position.NetQuantity - trade.Quantity < 0)
                {
                    continue;
                }

                position.NetQuantity -= trade.Quantity;
            }
        }

        return positions.Values
            .Where(position => position.NetQuantity != 0)
            .OrderBy(position => position.Symbol)
            .Select(PositionMapper.ToResponse)
            .ToList();
    }

    private void ValidateTrade(Trade trade)
    {
        if (trade is null)
        {
            throw new TradeValidationException("Trade is required for position calculation.");
        }

        if (trade.Quantity <= 0)
        {
            throw new TradeValidationException("Trade quantity must be greater than zero.");
        }
    }
}
