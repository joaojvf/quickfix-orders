namespace QuickFixOrders.Core.Data;

using QuickFixOrders.Core.Entities;

public interface IStockRepository
{
    Stock? GetStockBySymbol(string symbol);
    void SaveChanges();
}