using FluentValidation;
using TradeBlotterApi.ExceptionHandling;
using TradeBlotterApi.Data;
using TradeBlotterApi.Repositories;
using TradeBlotterApi.Services;
using TradeBlotterApi.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<TradeRequestValidator>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<ITradeRepository>(sp =>
{
    var dbContext = new TradeDbContext();
    dbContext.Database.EnsureCreated();
    return new TradeRepository(dbContext);
});
builder.Services.AddSingleton<IPositionService, PositionService>();
builder.Services.AddSingleton<ITradeService, TradeService>();

var app = builder.Build();

app.UseExceptionHandler();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();
