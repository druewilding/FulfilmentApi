using System.Threading.Channels;

class OrderProcessingWorker : BackgroundService
{
    private readonly Channel<Order> _channel;

    public OrderProcessingWorker(Channel<Order> channel)
    {
        _channel = channel;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await foreach (var order in _channel.Reader.ReadAllAsync(stoppingToken))
            {
                // Simulate order processing
                Console.WriteLine($"Processing order for product {order.ProductId} to be delivered at {order.DeliveryAddress}");
                // Here you would add your actual order processing logic
            };

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
