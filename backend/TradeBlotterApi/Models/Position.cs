namespace TradeBlotterApi.Models;

public class Position
{
    public string Symbol { get; set; } = string.Empty;
    public int NetQuantity { get; set; }
    public decimal AverageCost { get; set; }
}
