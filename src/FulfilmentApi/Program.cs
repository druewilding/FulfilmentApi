var orders = new Dictionary<Guid, Order>();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok(new { status = "healthy" }));

app.MapGet("/orders/{orderId:guid}", (Guid orderId) =>
{
    if (orders.TryGetValue(orderId, out var order))
    {
        return Results.Ok(order);
    }
    return Results.NotFound();
}).Produces<Order>(200).Produces(404);

app.MapPost("/orders", (Order order) =>
{
    // Process the order here
    var orderId = Guid.NewGuid();
    orders[orderId] = order;
    var orderResponse = new OrderResponse(orderId, "pending");
    return Results.Created($"/orders/{orderId}", orderResponse);
}).Produces<OrderResponse>(201);

app.Run();

record Order(Guid ProductId, int Quantity, string DeliveryAddress);
record OrderResponse(Guid OrderId, string Status);
