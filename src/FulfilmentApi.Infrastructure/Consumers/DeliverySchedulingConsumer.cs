namespace FulfilmentApi.Infrastructure.Consumers;

using MassTransit;
using FulfilmentApi.Domain;

public class DeliverySchedulingConsumer : IConsumer<OrderPlaced>
{
    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        var orderPlaced = context.Message;

        Console.WriteLine($"Scheduling delivery for Order ID: {orderPlaced.OrderId}");

        return Task.CompletedTask;
    }
}
