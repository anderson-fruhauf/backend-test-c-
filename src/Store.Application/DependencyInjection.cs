namespace Store.Application;

using Catalog.Queries.GetProducts;
using Microsoft.Extensions.DependencyInjection;
using Orders.Commands.AddProductToOrder;
using Orders.Commands.CloseOrder;
using Orders.Commands.CreateOrder;
using Orders.Commands.RemoveProductFromOrder;
using Orders.Queries.GetOrder;
using Orders.Queries.GetOrders;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<AddProductToOrderHandler>();
        services.AddScoped<RemoveProductFromOrderHandler>();
        services.AddScoped<CloseOrderHandler>();
        services.AddScoped<GetOrderHandler>();
        services.AddScoped<GetOrdersHandler>();
        services.AddScoped<GetProductsHandler>();

        return services;
    }
}
