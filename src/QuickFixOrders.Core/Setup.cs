namespace QuickFixOrders.Core;

using Microsoft.Extensions.DependencyInjection;
using QuickFixOrders.Core.UseCases.ExecuteOrder;

public static class Setup
{
    public static void AddDependencyInjectionCore(this IServiceCollection services)
    {
        services.AddScoped<IExecuteOrderHandler, ExecuteOrderHandler>();
    }
}