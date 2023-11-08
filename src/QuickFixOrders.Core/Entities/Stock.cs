namespace QuickFixOrders.Core.Entities;
public class Stock
{
    public int Id { get; set; }

    public string Symbol { get; set; } = null!;

    public decimal FinancialExposition { get; set; }

    public decimal FinancialExpositionBuyLimit { get; set; }

    public decimal FinancialExpositionSellLimit { get; set; }
}