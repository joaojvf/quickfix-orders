namespace QuickFixOrders.Core;

using Microsoft.Extensions.DependencyInjection;
using QuickFixOrders.Core.UseCases.ExecuteOrder;

public static class DependencyInjection
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddScoped<IExecuteOrderHandler, ExecuteOrderHandler>();
    }
}