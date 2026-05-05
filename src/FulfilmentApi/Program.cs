using System.Threading.Channels;
using FulfilmentApi.Domain;

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
        return Results.Ok(new OrderResponse(order.Id, order.Status.ToString()));
    }
    return Results.NotFound();
}).Produces<OrderResponse>(200).Produces(404);

app.MapPost("/orders", async (CreateOrderRequest orderRequest, IOrderService orderService) =>
{
    var order = orderService.CreateOrder(orderRequest);
    await channel.Writer.WriteAsync(order);
    return Results.Created($"/orders/{order.Id}", new OrderResponse(order.Id, order.Status.ToString()));
}).Produces<OrderResponse>(201);

app.Run();
