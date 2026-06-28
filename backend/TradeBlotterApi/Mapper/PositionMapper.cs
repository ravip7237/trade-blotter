using TradeBlotterApi.Dtos;
using TradeBlotterApi.Models;

namespace TradeBlotterApi.Mapper;

public static class PositionMapper
{
    public static PositionResponse ToResponse(Position position) => new()
    {
        Symbol = position.Symbol,
        NetQuantity = position.NetQuantity,
        AverageCost = position.AverageCost
    };
}
