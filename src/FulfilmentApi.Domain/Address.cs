namespace FulfilmentApi.Domain;

public record Address
{
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
};
