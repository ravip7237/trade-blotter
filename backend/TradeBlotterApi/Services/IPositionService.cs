using TradeBlotterApi.Dtos;
using TradeBlotterApi.Models;

namespace TradeBlotterApi.Services;

public interface IPositionService
{
    Task<IReadOnlyList<PositionResponse>> GetPositionsAsync();
    IReadOnlyList<PositionResponse> CalculatePositions(IEnumerable<Trade> trades);
}