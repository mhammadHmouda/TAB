using TAB.Domain.Core.Primitives;

namespace TAB.Domain.Features.HotelManagement.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency) => new(amount, currency.ToUpper());

    public static Money operator +(Money money1, Money money2)
    {
        if (money1.Currency != money2.Currency)
        {
            throw new InvalidOperationException("Cannot sum amounts with different currencies");
        }

        return Create(money1.Amount + money2.Amount, money1.Currency);
    }

    public static Money operator -(Money money1, Money money2)
    {
        if (money1.Currency != money2.Currency)
        {
            throw new InvalidOperationException(
                "Cannot subtract amounts with different currencies"
            );
        }

        return Create(money1.Amount - money2.Amount, money1.Currency);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
}
