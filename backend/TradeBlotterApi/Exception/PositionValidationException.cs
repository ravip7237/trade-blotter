namespace TradeBlotterApi.ExceptionHandling;

public sealed class PositionValidationException : DomainException
{
    public PositionValidationException(string message)
        : base(message)
    {
    }
}