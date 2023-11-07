namespace QuickFixOrders.Core.Entities.Base;

using QuickFix.Fields;
using QuickFix.FIX44;

public abstract class OrderSymbol : NewOrderSingle
{
    private readonly Random _random = new();
    public virtual decimal FinancialExpositionLimit { get; } = 1000000m;
    protected OrderSymbol(ClOrdID clOrdId, Symbol symbol)
    {
        var side = this.GenerateSide();
        var quantity = this.GenerateQuantity();
        var price = this.GeneratePriceValue();

        this.ClOrdID = clOrdId;
        this.Symbol = symbol;
        this.Side = side;
        this.TransactTime = new TransactTime(DateTime.Now);
        this.OrdType = new OrdType(OrdType.MARKET);
        this.Price = price;
        this.OrderQty = quantity;
    }

    private OrderSymbol() { }

    public decimal FinancialExposition { get; set; }

    public virtual decimal TotalOrderPrice => this.Price.getValue() * this.OrderQty.getValue();

    public virtual bool IsExecutable()
    {
        return this.FinancialExposition + this.TotalOrderPrice >= this.FinancialExpositionLimit;
    }

    protected virtual Side GenerateSide() => _random.Next(1, 2) == 1 ? new Side(Side.BUY) : new Side(Side.SELL);

    protected virtual OrderQty GenerateQuantity() => new(this._random.Next(1, 100000));

    protected virtual Price GeneratePriceValue()
    {
        var price = (decimal)this._random.NextDouble() * (1000.0m - 0.01m) + 0.01m;
        return new Price(decimal.Round(price, 2));
    }
}