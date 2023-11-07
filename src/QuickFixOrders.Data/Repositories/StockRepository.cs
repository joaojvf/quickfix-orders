namespace QuickFixOrders.Data.Repositories;

using QuickFixOrders.Core.Data;
using QuickFixOrders.Core.Entities;

public class StockRepository : IStockRepository
{
    private readonly DataContext _context;
    public StockRepository(DataContext context)
    {
        this._context = context;
    }
    public Stock? GetStockBySymbol(string symbol)
    {
        return this._context.Stocks.FirstOrDefault(x => x.Symbol.Equals(symbol, StringComparison.CurrentCultureIgnoreCase));
    }
    public void SaveChanges()
    {
        this._context.SaveChanges();
    }
}