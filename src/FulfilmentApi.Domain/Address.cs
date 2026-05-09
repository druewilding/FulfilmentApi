namespace FulfilmentApi.Domain;

public record Address
{
    public string Street { get; init; }
    public string PostalCode { get; init; }
    public string City { get; init; }

    public Address(string street, string postalCode, string city)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty.", nameof(street));
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("PostalCode cannot be empty.", nameof(postalCode));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty.", nameof(city));

        Street = street;
        PostalCode = postalCode;
        City = city;
    }
};
