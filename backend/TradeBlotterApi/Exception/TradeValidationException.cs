namespace TradeBlotterApi.ExceptionHandling;

public sealed class TradeValidationException : DomainException
{
    public TradeValidationException(string message)
        : base(message)
    {
    }
}