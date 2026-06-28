using System;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

using TradeBlotterApi.Dtos;
using TradeBlotterApi.Services;

namespace TradeBlotterApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TradesController : ControllerBase
{
    private readonly ITradeService _tradeService;
    private readonly IPositionService _positionService;
    private readonly IValidator<TradeRequest> _validator;

    public TradesController(
        ITradeService tradeService,
        IPositionService positionService,
        IValidator<TradeRequest> validator)
    {
        _tradeService = tradeService;
        _positionService = positionService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult<TradeResponse>> CreateTrade([FromBody] TradeRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(
                validationResult.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.ErrorMessage).Distinct(StringComparer.Ordinal).ToArray()))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Trade request validation failed.",
            });
        }

        var createdTrade = await _tradeService.AddTradeAsync(request);

        return CreatedAtAction(nameof(CreateTrade), new { id = createdTrade.Id }, createdTrade);
    }


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TradeResponse>>> GetAllTrades()
    {
        var trades = await _tradeService.GetAllTradesAsync();
        return Ok(trades.ToList());
    }

    [HttpGet("/positions")]
    public async Task<ActionResult<IReadOnlyList<PositionResponse>>> GetPositions()
    {
        var positions = await _positionService.GetPositionsAsync();
        return Ok(positions.ToList());
    }

}
