using TradeBlotterApi.Dtos;
using TradeBlotterApi.Enum;
using TradeBlotterApi.ExceptionHandling;
using TradeBlotterApi.Models;
using TradeBlotterApi.Repositories;
using TradeBlotterApi.Services;
using Xunit;

namespace TradeBlotterApi.Tests.Services;

public class TradeServiceTests
{
    [Fact]
    public async Task AddTradeAsync_Throws_WhenSellExceedsCurrentPosition()
    {
        var repository = new RecordingTradeRepository();
        var positionService = new StubPositionService(new PositionResponse
        {
            Symbol = "AAPL",
            NetQuantity = 5,
            AverageCost = 100m
        });
        var service = new TradeService(repository, positionService);

        var request = new TradeRequest
        {
            Symbol = "AAPL",
            Side = "Sell",
            Quantity = 10,
            Price = 101m,
            Timestamp = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };

        var exception = await Assert.ThrowsAsync<PositionValidationException>(() => service.AddTradeAsync(request));

        Assert.Equal("Sell quantity cannot exceed the current position.", exception.Message);
        Assert.Empty(repository.AddedTrades);
    }

    [Fact]
    public async Task AddTradeAsync_PersistsSell_WhenPositionExists()
    {
        var repository = new RecordingTradeRepository();
        var positionService = new StubPositionService(new PositionResponse
        {
            Symbol = "AAPL",
            NetQuantity = 12,
            AverageCost = 100m
        });
        var service = new TradeService(repository, positionService);

        var request = new TradeRequest
        {
            Symbol = "AAPL",
            Side = "Sell",
            Quantity = 5,
            Price = 101m,
            Timestamp = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };

        var response = await service.AddTradeAsync(request);

        Assert.Equal("AAPL", response.Symbol);
        Assert.Equal("Sell", response.Side);
        Assert.Single(repository.AddedTrades);
        Assert.Equal(5, repository.AddedTrades[0].Quantity);
    }

    private sealed class RecordingTradeRepository : ITradeRepository
    {
        public List<Trade> AddedTrades { get; } = [];

        public Task<Trade> AddTradeAsync(Trade trade)
        {
            AddedTrades.Add(trade);
            trade.Id = AddedTrades.Count;
            return Task.FromResult(trade);
        }

        public Task<IReadOnlyList<Trade>> GetAllTradesAsync() => Task.FromResult<IReadOnlyList<Trade>>(AddedTrades);
    }

    private sealed class StubPositionService : IPositionService
    {
        private readonly IReadOnlyList<PositionResponse> _positions;

        public StubPositionService(params PositionResponse[] positions)
        {
            _positions = positions;
        }

        public Task<IReadOnlyList<PositionResponse>> GetPositionsAsync() => Task.FromResult(_positions);

        public IReadOnlyList<PositionResponse> CalculatePositions(IEnumerable<Trade> trades) => _positions;
    }
}