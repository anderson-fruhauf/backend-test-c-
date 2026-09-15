namespace Store.Application;

using Microsoft.Extensions.DependencyInjection;
using Orders.Commands.CreateOrder;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateOrderHandler>();

        return services;
    }
}
