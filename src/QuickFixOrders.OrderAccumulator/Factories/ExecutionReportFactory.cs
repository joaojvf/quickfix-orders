namespace QuickFixOrders.OrderAccumulator;

using QuickFix.Fields;
using QuickFix.FIX44;

public static class ExecutionReportFactory
{
    private static int _orderId;
    private static int _execId;

    public static  QuickFix.FIX44.ExecutionReport CreateExecutionReport44(NewOrderSingle newOrderSingle, bool executed)
    {
        var execType = CreateExecType(executed);
        var ordStatus = CreateOrdStatus(executed);

        var report = new QuickFix.FIX44.ExecutionReport(
            new OrderID(CreateOrderId()),
            new ExecID(CreateExecId()),
            execType,
            ordStatus,
            newOrderSingle.Symbol,
            newOrderSingle.Side,
            new LeavesQty(0),
            new CumQty(newOrderSingle.OrderQty.getValue()),
            new AvgPx(newOrderSingle.Price.getValue()));

        report.Set(newOrderSingle.ClOrdID);
        report.Set(newOrderSingle.Symbol);
        report.Set(newOrderSingle.OrderQty);
        report.Set(new LastQty(newOrderSingle.OrderQty.getValue()));
        report.Set(new LastPx(newOrderSingle.Price.getValue()));

        if (newOrderSingle.IsSetAccount())
            newOrderSingle.SetField(newOrderSingle.Account);

        return report;
    }

    private static string CreateOrderId() => (++_orderId).ToString();
    private static string CreateExecId() => (++_execId).ToString();
    private static OrdStatus CreateOrdStatus(bool executed) => executed ? new OrdStatus(OrdStatus.FILLED) : new OrdStatus(OrdStatus.REJECTED);
    private static ExecType CreateExecType(bool executed) => executed ? new ExecType(ExecType.FILL) : new ExecType(ExecType.REJECTED);
}