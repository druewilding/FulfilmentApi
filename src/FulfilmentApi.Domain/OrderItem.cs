namespace FulfilmentApi.Domain;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; init; }
    public Weight Weight { get; init; }

    public OrderItem(Guid productId, Weight weight)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("ProductId cannot be empty.", nameof(productId));
        ArgumentNullException.ThrowIfNull(weight);

        Id = Guid.NewGuid();
        ProductId = productId;
        Weight = weight;
    }
}
