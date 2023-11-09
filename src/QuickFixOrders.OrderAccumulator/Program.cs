using Acceptor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickFixOrders.OrderAccumulator;
using QuickFix;
using QuickFixOrders.Core;
using QuickFixOrders.Data;

class Program
{
    static void Main(string[] args)
    {
        const string httpServerPrefix = "http://127.0.0.1:5080/";
        var combinedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orderAccumulator.cfg");
        var app = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDependencyInjectionCore();
                services.AddDependencyInjectionData();
                services.AddSingleton<OrderAccumulator>();
            }).Build();
        QuickFixOrders.Data.Setup.AddInitialData(app.Services.GetRequiredService<DataContext>());

        try
        {
            SessionSettings settings = new SessionSettings(combinedPath);
            IApplication executorApp = app.Services.GetRequiredService<OrderAccumulator>();
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
            ILogFactory logFactory = new FileLogFactory(settings);
            ThreadedSocketAcceptor acceptor = new ThreadedSocketAcceptor(executorApp, storeFactory, settings, logFactory);
            HttpServer srv = new HttpServer(httpServerPrefix, settings);
            
            Console.WriteLine("Starting Order Accumulator...");
            acceptor.Start();
            srv.Start();
            Console.WriteLine("Order Accumulator started!");
        }
        catch (Exception e)
        {
            Console.WriteLine("==FATAL ERROR==");
            Console.WriteLine(e.ToString());
        }
    }
}
