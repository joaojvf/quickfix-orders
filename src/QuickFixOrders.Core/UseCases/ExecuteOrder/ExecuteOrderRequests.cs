namespace QuickFixOrders.Core.UseCases.ExecuteOrder;

using QuickFix.FIX44;

public record ExecuteOrderRequest(NewOrderSingle OrderSingle);