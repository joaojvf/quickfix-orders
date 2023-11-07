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
        var order = this._stockRepository.GetStockBySymbol(request.OrderSingle.Symbol.getValue());

        if (order is null)
        {
            return new ExecuteOrderResponse(false);
        }

        var price = request.OrderSingle.Price.getValue();
        var quantity = Convert.ToInt32(request.OrderSingle.OrderQty.getValue());
        var totalToExecute = price * quantity;

        return request.OrderSingle.Side.getValue() switch
        {
            Side.BUY => this.ExecuteBuy(order, totalToExecute),
            Side.SELL => this.ExecuteSell(order, totalToExecute),
            _ => new ExecuteOrderResponse(false)
        };
    }

    private ExecuteOrderResponse ExecuteSell(Stock order, decimal totalToExecute)
    {
        var newFinancialExposition = Calc();
        order.FinancialExposition = newFinancialExposition;

        this._stockRepository.SaveChanges();
        return new ExecuteOrderResponse(true);

        decimal Calc()
        {
            return order.FinancialExposition - totalToExecute;
        }
    }

    private ExecuteOrderResponse ExecuteBuy(Stock order, decimal totalToExecute)
    {
        var newFinancialExposition = Calc();

        if (!IsExecutable(order, newFinancialExposition))
        {
            return new ExecuteOrderResponse(false);
        }

        order.FinancialExposition = newFinancialExposition;
        this._stockRepository.SaveChanges();
        return new ExecuteOrderResponse(true);

        decimal Calc()
        {
            return order.FinancialExposition + totalToExecute;
        }
    }

    private bool IsExecutable(Stock order, decimal newFinancialExposition)
    {
        return newFinancialExposition + order.FinancialExposition <= order.FinancialExpositionLimit;
    }
}