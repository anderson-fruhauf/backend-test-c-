namespace Store.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Total => UnitPrice * Quantity;

    private OrderItem() { }

    public OrderItem(Guid id, Guid productId, string productName, int quantity, decimal price)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }

        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product id cannot be empty.", nameof(productId));
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new ArgumentException("Product name cannot be empty.", nameof(productName));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than 0.");
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Unit price cannot be negative.");
        }

        Id = id;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = price;
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than 0.");
        }

        Quantity += quantity;
    }
}