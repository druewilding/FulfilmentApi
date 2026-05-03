public interface IOrderService
{
    OrderResponse CreateOrder(Order order);
    Order? GetOrder(Guid orderId);
}

public class OrderService : IOrderService
{
    private readonly Dictionary<Guid, Order> orders = new();

    public OrderResponse CreateOrder(Order order)
    {
        var orderId = Guid.NewGuid();
        orders[orderId] = order;
        return new OrderResponse(orderId, "pending");
    }

    public Order? GetOrder(Guid orderId)
    {
        if (orders.TryGetValue(orderId, out var order))
        {
            return order;
        }
        return null;
    }
}

public record Order(Guid ProductId, int Quantity, string DeliveryAddress);
public record OrderResponse(Guid OrderId, string Status);
