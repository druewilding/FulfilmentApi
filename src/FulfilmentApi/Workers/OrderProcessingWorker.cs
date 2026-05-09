using System.Threading.Channels;
using FulfilmentApi.Domain;

class OrderProcessingWorker(Channel<Order> channel) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var order in channel.Reader.ReadAllAsync(stoppingToken))
        {
            // Simulate order processing
            Console.WriteLine($"Processing order to be delivered at {order.DeliveryAddress}");
            // Here you would add your actual order processing logic
        }
    }
}
