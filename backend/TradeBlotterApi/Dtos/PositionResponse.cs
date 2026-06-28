namespace TradeBlotterApi.Dtos;

public class PositionResponse
{
    public string Symbol { get; set; } = string.Empty;
    public int NetQuantity { get; set; }
    public decimal AverageCost { get; set; }
}
