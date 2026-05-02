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

app.MapPost("/orders", (Order order) =>
{
    // Process the order here
    var orderId = Guid.NewGuid();
    return Results.Created($"/orders/{orderId}", new { status = "pending" });
}).Produces<OrderResponse>(201);

app.Run();

record Order(Guid ProductId, int Quantity, string DeliveryAddress);
record OrderResponse(Guid OrderId, string Status);
