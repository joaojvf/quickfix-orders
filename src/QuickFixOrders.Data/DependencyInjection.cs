namespace QuickFixOrders.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuickFixOrders.Core.Constants;
using QuickFixOrders.Core.Data;
using QuickFixOrders.Core.Entities;
using QuickFixOrders.Data.Repositories;

public static class DependencyInjection
{
    public static void AddData(this IServiceCollection services)
    {
        services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase(Application.DatabaseName));
        services.AddScoped<IStockRepository, StockRepository>();
    }

    public static void AddInitialData(DataContext context)
    {
        var list = new Stock[]
        {
            new()
                { Id = 1, Symbol = Symbols.PETR4, FinancialExposition = 0, FinancialExpositionLimit = 1000000m },
            new()
                { Id = 2, Symbol = Symbols.VALE3, FinancialExposition = 0, FinancialExpositionLimit = 1000000m },
            new()
                { Id = 3, Symbol = Symbols.VIIA4, FinancialExposition = 0, FinancialExpositionLimit = 1000000m }
        };

        context.Stocks.AddRange(list);
        context.SaveChanges();
    }
}