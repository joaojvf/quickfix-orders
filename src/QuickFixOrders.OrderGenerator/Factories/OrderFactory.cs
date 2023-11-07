namespace QuickFixOrders.OrderGenerator.Factories;

using QuickFix.Fields;
using QuickFix.FIX44;

public static class OrderFactory
{
    private static int _orderId;
    private static readonly Random Random = new();

    public static QuickFix.FIX44.NewOrderSingle CreateOrder44(string symbol)
    {
        var side = CreateSide();
        var quantity = CreateQuantity();
        var price = CreatePriceValue();
        return new NewOrderSingle(new ClOrdID(CreateOrderId()), new Symbol(symbol), side, new TransactTime(DateTime.Now), new OrdType(OrdType.MARKET))
        {
            Price = price,
            OrderQty = quantity
        };
    }

    private static string CreateOrderId() => (++_orderId).ToString();
    private static Side CreateSide() => Random.Next(1, 2) == 1 ? new Side(Side.BUY) : new Side(Side.SELL);
    private static OrderQty CreateQuantity() => new(Random.Next(1, 100000));
    private static Price CreatePriceValue()
    {
        var price = (decimal)Random.NextDouble() * (1.0m - 0.01m) + 0.01m;
        return new Price(decimal.Round(price, 2));
    }
}