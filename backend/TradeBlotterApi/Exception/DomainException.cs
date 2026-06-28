namespace TradeBlotterApi.ExceptionHandling;

public abstract class DomainException : Exception
{
    protected DomainException(string message)
        : base(message)
    {
    }
}