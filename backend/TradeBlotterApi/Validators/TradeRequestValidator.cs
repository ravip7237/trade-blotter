using FluentValidation;
using TradeBlotterApi.Dtos;
using TradeBlotterApi.Enum;

namespace TradeBlotterApi.Validators;

public class TradeRequestValidator : AbstractValidator<TradeRequest>
{
    public TradeRequestValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("Request body is required.");

        RuleFor(x => x!.Symbol)
            .Must(symbol => !string.IsNullOrWhiteSpace(symbol))
            .WithMessage("Symbol is required.");

        RuleFor(x => x!.Side)
            .Must(side => !string.IsNullOrWhiteSpace(side))
            .WithMessage("Side is required.");

        RuleFor(x => x!.Side)
            .Must(side => System.Enum.TryParse<TradeSide>(side, true, out _))
            .WithMessage("Side must be Buy or Sell.");

        RuleFor(x => x!.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x!.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");
    }
}
