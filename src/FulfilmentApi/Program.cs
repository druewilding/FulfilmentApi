using System.Threading.Channels;
using FulfilmentApi.Domain;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

var channel = Channel.CreateUnbounded<Order>();
builder.Services.AddSingleton(channel);
builder.Services.AddHostedService<OrderProcessingWorker>(provider => new OrderProcessingWorker(channel));

// Configure MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapGet("/", () => Results.Ok(new { status = "healthy" }));

app.MapGet("/orders/{orderId:guid}", async (Guid orderId, IOrderService orderService) =>
{
    var order = await orderService.GetOrder(orderId);
    if (order != null)
    {
        return Results.Ok(new OrderResponse(order.Id, order.Status.ToString()));
    }
    return Results.NotFound();
}).Produces<OrderResponse>(200).Produces(404);

app.MapPost("/orders", async (CreateOrderRequest orderRequest, IOrderService orderService) =>
{
    var order = await orderService.CreateOrder(orderRequest);
    await channel.Writer.WriteAsync(order);
    return Results.Created($"/orders/{order.Id}", new OrderResponse(order.Id, order.Status.ToString()));
}).Produces<OrderResponse>(201);

app.Run();
