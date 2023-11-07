using QuickFixOrders.OrderGenerator;
using QuickFix;
using QuickFix.Transport;

var combinedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orderGenerator.cfg");
try
{
    SessionSettings settings = new SessionSettings(combinedPath);
    OrderGenerator application = new OrderGenerator();
    IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
    ILogFactory logFactory = new ScreenLogFactory(settings);
    SocketInitiator initiator = new SocketInitiator(application, storeFactory, settings, logFactory);

    initiator.Start();
    application.Run();
    initiator.Stop();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.StackTrace);
}