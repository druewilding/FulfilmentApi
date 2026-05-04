using System.Threading.Channels;

class OrderProcessingWorker(Channel<Order> channel) : BackgroundService
{
    private readonly Channel<Order> _channel = channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var order in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            // Simulate order processing
            Console.WriteLine($"Processing order for product {order.ProductId} to be delivered at {order.DeliveryAddress}");
            // Here you would add your actual order processing logic
        }
    }
}
