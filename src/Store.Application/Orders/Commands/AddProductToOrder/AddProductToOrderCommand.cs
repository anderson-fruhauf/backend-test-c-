namespace Store.Application.Orders.Commands.AddProductToOrder;

public record AddProductToOrderCommand(Guid OrderId, Guid ProductId, int Quantity);
