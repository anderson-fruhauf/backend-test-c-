namespace Store.Application;

using Microsoft.Extensions.DependencyInjection;
using Orders.Commands.CreateOrder;
using Orders.Queries.GetOrder;
using Orders.Queries.GetOrders;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<GetOrderHandler>();
        services.AddScoped<GetOrdersHandler>();

        return services;
    }
}
