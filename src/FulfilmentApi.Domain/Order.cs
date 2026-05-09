namespace FulfilmentApi.Domain;

public enum OrderStatus
{
    Pending,
    Processing,
    Completed,
    Cancelled
}

public class Order
{
    private static readonly Weight MaxWeight = new Weight(30_000, Unit.Grams);

    public Guid Id { get; private set; }
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public Address DeliveryAddress { get; private set; }
    public OrderStatus Status { get; private set; }

    public Order(Guid productId, int quantity, Address deliveryAddress)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        DeliveryAddress = deliveryAddress;
        Status = OrderStatus.Pending;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed.");

        Status = OrderStatus.Processing;
    }
}
