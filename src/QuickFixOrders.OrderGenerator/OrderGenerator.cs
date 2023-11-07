namespace QuickFixOrders.OrderGenerator;

using QuickFix;
using QuickFixOrders.Core.Constants;
using QuickFixOrders.OrderGenerator.Factories;
using Message = QuickFix.Message;

public class OrderGenerator : MessageCracker, IApplication
{
    private readonly Random _random = new();
    private Session _session;

    private const int MillisecondsInterval = 10000;
    private static readonly string[] EnabledSymbols = { Symbols.PETR4, Symbols.VALE3, Symbols.VIIA4 };

    public void OnCreate(SessionID sessionID)
    {
        _session = Session.LookupSession(sessionID);
    }

    public void OnLogon(SessionID sessionID) { }
    public void OnLogout(SessionID sessionID) { }
    public void FromAdmin(Message message, SessionID sessionID) { }
    public void ToAdmin(Message message, SessionID sessionID) { }
    public void ToApp(Message message, SessionID sessionID) { }

    public void FromApp(Message message, SessionID sessionID)
    {
        try
        {
            Crack(message, sessionID);
        }
        catch (Exception ex)
        {
            Console.WriteLine("==Cracker exception==");
            Console.WriteLine(ex.ToString());
            Console.WriteLine(ex.StackTrace);
        }
    }

    public void OnMessage(QuickFix.FIX44.ExecutionReport m, SessionID s)
    {
        Console.WriteLine($"OrderGenerator - Execution Report: {m.ExecID}, OrderId: {m.OrderID}, Symbol: {m.Symbol}, Status: {m.OrdStatus}.");
    }

    public void Run()
    {
        while (true)
        {
            var order = OrderFactory.CreateOrder44(GetRandomSymbol());
            Session.SendToTarget(order, this._session.SessionID);
            WaitForInterval();
        }
    }

    private string GetRandomSymbol()
    {
        var arrayPosition = this._random.Next(0, EnabledSymbols.Length - 1);
        return EnabledSymbols[arrayPosition];
    }

    private void WaitForInterval() =>
        Thread.Sleep(MillisecondsInterval);
}