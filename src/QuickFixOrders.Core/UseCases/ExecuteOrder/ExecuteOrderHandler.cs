namespace QuickFixOrders.Core.UseCases.ExecuteOrder;

using QuickFix.Fields;
using QuickFixOrders.Core.Data;
using QuickFixOrders.Core.Entities;

public class ExecuteOrderHandler : IExecuteOrderHandler
{
    private readonly IStockRepository _stockRepository;

    public ExecuteOrderHandler(IStockRepository stockRepository)
    {
        this._stockRepository = stockRepository;
    }

    public ExecuteOrderResponse Handle(ExecuteOrderRequest request)
    {
        var stock = this._stockRepository.GetStockBySymbol(request.OrderSingle.Symbol.getValue());

        if (stock is null)
        {
            return new ExecuteOrderResponse(false);
        }

        var price = request.OrderSingle.Price.getValue();
        var quantity = Convert.ToInt32(request.OrderSingle.OrderQty.getValue());
        var totalToExecute = GetTotalValue(price, quantity);

        return request.OrderSingle.Side.getValue() switch
        {
            Side.BUY => this.ExecuteBuy(stock, totalToExecute),
            Side.SELL => this.ExecuteSell(stock, totalToExecute),
            _ => new ExecuteOrderResponse(false)
        };
    }

    private ExecuteOrderResponse ExecuteSell(Stock stock, decimal totalToExecute)
    {
        var newFinancialExposition = Calc();

        if (!SellExecutable(stock, newFinancialExposition))
        {
            return new ExecuteOrderResponse(false);
        }

        stock.FinancialExposition = newFinancialExposition;
        this._stockRepository.SaveChanges();
        return new ExecuteOrderResponse(true);

        decimal Calc()
        {
            return stock.FinancialExposition - totalToExecute;
        }
    }

    private ExecuteOrderResponse ExecuteBuy(Stock stock, decimal totalToExecute)
    {

        var newFinancialExposition = Calc();
        if (!BuyExecutable(stock, newFinancialExposition))
        {
            return new ExecuteOrderResponse(false);
        }

        stock.FinancialExposition = newFinancialExposition;
        this._stockRepository.SaveChanges();
        return new ExecuteOrderResponse(true);

        decimal Calc()
        {
            return stock.FinancialExposition + totalToExecute;
        }
    }

    private bool BuyExecutable(Stock stock, decimal newFinancialExposition) => newFinancialExposition <= stock.FinancialExpositionBuyLimit;

    private bool SellExecutable(Stock stock, decimal newFinancialExposition) => newFinancialExposition >= stock.FinancialExpositionSellLimit;

    private decimal GetTotalValue(decimal price, int quantity) => price * quantity;
}