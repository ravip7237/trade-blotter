using Microsoft.EntityFrameworkCore;
using TradeBlotterApi.Models;

namespace TradeBlotterApi.Data;

public class TradeDbContext : DbContext
{
    private readonly string _databasePath;

    public TradeDbContext(string? databasePath = null)
    {
        _databasePath = databasePath ?? Path.Combine(AppContext.BaseDirectory, "tradeblotter.db");
    }

    public DbSet<Trade> Trades => Set<Trade>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_databasePath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trade>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Symbol).IsRequired().HasMaxLength(20);
            entity.Property(t => t.Side).HasConversion<int>().IsRequired();
            entity.Property(t => t.Quantity).IsRequired();
            entity.Property(t => t.Price).HasColumnType("TEXT").IsRequired();
            entity.Property(t => t.Timestamp).IsRequired();
        });
    }
}
