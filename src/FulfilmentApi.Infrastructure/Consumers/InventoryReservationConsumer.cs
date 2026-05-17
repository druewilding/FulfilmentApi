namespace FulfilmentApi.Infrastructure.Consumers;

using MassTransit;
using FulfilmentApi.Domain;

public class InventoryReservationConsumer : IConsumer<OrderPlaced>
{
    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        var orderPlaced = context.Message;

        Console.WriteLine($"Reserving inventory for Order ID: {orderPlaced.OrderId}");

        return Task.CompletedTask;
    }
}
