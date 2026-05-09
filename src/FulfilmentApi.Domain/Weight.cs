namespace FulfilmentApi.Domain;

public enum Unit
{
    None = 0,
    Kilograms,
    Grams,
}

public record Weight
{
    public decimal Amount { get; init; }
    public Unit Unit { get; init; }

    public Weight(decimal amount, Unit unit)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        if (unit == Unit.None)
            throw new ArgumentException("Unit must be specified.", nameof(unit));

        Amount = amount;
        Unit = unit;
    }

    public override string ToString() => $"{Amount} {Unit}";

    public static Weight operator +(Weight a, Weight b)
    {
        if (a.Unit != b.Unit)
            throw new InvalidOperationException("Cannot add amounts with different units.");

        return new Weight(a.Amount + b.Amount, a.Unit);
    }

    public static Weight operator -(Weight a, Weight b)
    {
        if (a.Unit != b.Unit)
            throw new InvalidOperationException("Cannot subtract amounts with different units.");

        return new Weight(a.Amount - b.Amount, a.Unit);
    }

    public static bool operator >(Weight a, Weight b)
    {
        if (a.Unit != b.Unit)
            throw new InvalidOperationException("Cannot compare amounts with different units.");

        return a.Amount > b.Amount;
    }

    public static bool operator <(Weight a, Weight b)
    {
        if (a.Unit != b.Unit)
            throw new InvalidOperationException("Cannot compare amounts with different units.");

        return a.Amount < b.Amount;
    }
};
