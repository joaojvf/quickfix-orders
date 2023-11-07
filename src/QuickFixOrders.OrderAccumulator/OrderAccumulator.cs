namespace QuickFixOrders.OrderAccumulator;

using QuickFix;
using QuickFixOrders.Core.UseCases.ExecuteOrder;
using Message = QuickFix.Message;

public class OrderAccumulator : MessageCracker, IApplication
{
    private readonly IExecuteOrderHandler _executeOrderHandler;

    public OrderAccumulator(IExecuteOrderHandler executeOrderHandler)
    {
        this._executeOrderHandler = executeOrderHandler;
    }

    public void OnCreate(SessionID sessionID) { }
    public void OnLogout(SessionID sessionID) { }
    public void OnLogon(SessionID sessionID) { }
    public void ToAdmin(Message message, SessionID sessionID) { }
    public void FromAdmin(Message message, SessionID sessionID) { }
    public void ToApp(Message message, SessionID sessionID) { }

    public void FromApp(Message message, SessionID sessionID)
    {
        Crack(message, sessionID);
    }

    public void OnMessage(QuickFix.FIX44.NewOrderSingle newOrderSingle, SessionID s)
    {
        try
        {
            var response = this._executeOrderHandler.Handle(new ExecuteOrderRequest(newOrderSingle));
            var message = ExecutionReportFactory.CreateExecutionReport44(newOrderSingle, response.Executed);
            Session.SendToTarget(message, s);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}