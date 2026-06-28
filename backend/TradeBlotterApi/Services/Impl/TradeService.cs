using TradeBlotterApi.Dtos;
using TradeBlotterApi.Enum;
using TradeBlotterApi.ExceptionHandling;
using TradeBlotterApi.Mapper;
using TradeBlotterApi.Models;
using TradeBlotterApi.Repositories;

namespace TradeBlotterApi.Services;

public class TradeService : ITradeService
{
    private readonly ITradeRepository _repository;
    private readonly IPositionService _positionService;

    public TradeService(ITradeRepository repository, IPositionService positionService)
    {
        _repository = repository;
        _positionService = positionService;
    }

    public async Task<TradeResponse> AddTradeAsync(TradeRequest request)
    {
        if (request is null)
        {
            throw new TradeValidationException("Trade request is required.");
        }

        var trade = TradeMapper.ToModel(request);

        if (trade.Side == TradeSide.Sell)
        {
            var positions = await _positionService.GetPositionsAsync();
            var currentPosition = positions.FirstOrDefault(position =>
                string.Equals(position.Symbol, trade.Symbol, StringComparison.OrdinalIgnoreCase));

            if (currentPosition is null || trade.Quantity > currentPosition.NetQuantity)
            {
                throw new PositionValidationException("Sell quantity cannot exceed the current position.");
            }
        }

        var createdTrade = await _repository.AddTradeAsync(trade);
        return TradeMapper.ToResponse(createdTrade);
    }

    public async Task<IReadOnlyList<TradeResponse>> GetAllTradesAsync()
    {
        var trades = await _repository.GetAllTradesAsync();
        return trades.Select(TradeMapper.ToResponse).ToList();
    }
}
