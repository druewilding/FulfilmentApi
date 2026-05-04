using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IOrderService, OrderService>();

var channel = Channel.CreateUnbounded<Order>();
builder.Services.AddSingleton(channel);
builder.Services.AddHostedService<OrderProcessingWorker>(provider => new OrderProcessingWorker(channel));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapGet("/", () => Results.Ok(new { status = "healthy" }));

app.MapGet("/orders/{orderId:guid}", (Guid orderId, IOrderService orderService) =>
{
    var order = orderService.GetOrder(orderId);
    if (order != null)
    {
        return Results.Ok(order);
    }
    return Results.NotFound();
}).Produces<Order>(200).Produces(404);

app.MapPost("/orders", (Order order, IOrderService orderService) =>
{
    channel.Writer.WriteAsync(order);
    var orderResponse = orderService.CreateOrder(order);
    return Results.Created($"/orders/{orderResponse.OrderId}", orderResponse);
}).Produces<OrderResponse>(201);

app.Run();
