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
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
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
