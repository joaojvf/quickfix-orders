namespace QuickFixOrders.Data;

using Microsoft.EntityFrameworkCore;
using QuickFixOrders.Core.Entities;

public class DataContext : DbContext
{
    public DbSet<Stock> Stocks { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stock>(x =>
        {
            x.HasKey(p => p.Id);
            x.Property(p => p.Id).ValueGeneratedOnAdd();
            x.HasIndex(p => p.Symbol).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}