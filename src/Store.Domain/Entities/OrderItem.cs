namespace Store.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Total => UnitPrice * Quantity;

    public OrderItem(Guid id, Guid productId,string productName, int quantity, decimal price)
    {
        Id = id;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = price;
    }
}