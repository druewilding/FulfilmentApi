using FulfilmentApi.Domain;

public interface IOrderService
{
    Order CreateOrder(CreateOrderRequest orderRequest);
    Order? GetOrder(Guid orderId);
}

public class OrderService : IOrderService
{
    private readonly Dictionary<Guid, Order> orders = new();

    public Order CreateOrder(CreateOrderRequest orderRequest)
    {
        var order = new Order(orderRequest.ProductId, orderRequest.Quantity, orderRequest.DeliveryAddress);
        var orderId = order.Id;
        orders[orderId] = order;
        return order;
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

public record CreateOrderRequest(Guid ProductId, int Quantity, string DeliveryAddress);
