using FulfilmentApi.Domain;
using MassTransit;

public interface IOrderService
{
    Task<Order> CreateOrder(CreateOrderRequest orderRequest);
    Task<Order?> GetOrder(Guid orderId);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository orders;
    private readonly IPublishEndpoint publishEndpoint;

    public OrderService(IOrderRepository orders, IPublishEndpoint publishEndpoint)
    {
        this.orders = orders;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task<Order> CreateOrder(CreateOrderRequest orderRequest)
    {
        var deliveryAddress = new Address(orderRequest.DeliveryAddress.Street, orderRequest.DeliveryAddress.PostalCode, orderRequest.DeliveryAddress.City);
        var order = new Order(deliveryAddress);

        foreach (var itemRequest in orderRequest.Items)
        {
            var orderItem = new OrderItem(itemRequest.ProductId, new Weight(itemRequest.Weight.Amount, itemRequest.Weight.Unit));
            order.AddItem(orderItem);
        }

        var orderId = order.Id;
        await orders.Save(order);

        // Publish domain events to RabbitMQ
        await publishEndpoint.Publish(new OrderPlaced(orderId));

        Console.WriteLine($"Order created with ID: {orderId}, containing {order.Items.Count} items, total weight: {order.Items.Sum(i => i.Weight.Amount)} {order.Items.FirstOrDefault()?.Weight.Unit}");
        return order;
    }

    public async Task<Order?> GetOrder(Guid orderId)
    {
        return await orders.FindById(orderId);
    }
}
