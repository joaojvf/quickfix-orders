namespace QuickFixOrders.Core.UseCases.ExecuteOrder;

public interface IExecuteOrderHandler
{
    ExecuteOrderResponse Handle(ExecuteOrderRequest request);
}