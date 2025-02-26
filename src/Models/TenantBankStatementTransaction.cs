using System.Globalization;
using System.Text.RegularExpressions;

namespace AffordabilityServiceCore.Models;

public class TenantBankStatementTransaction(
    DateTime timestamp,
    string transactionType,
    string description,
    decimal delta,
    TransactionDirection direction,
    decimal balance)
{
    public DateTime Timestamp { get; init; } = timestamp;
    public DateTime TransactionMonth { get; init; } = new(timestamp.Year, timestamp.Month, 1);
    public string TransactionType { get; init; } = transactionType;
    public string Description { get; init; } = description;
    public decimal Delta { get; init; } = delta;
    public TransactionDirection Direction { get; init; } = direction;
    public decimal Balance { get; init; } = balance;

    public static TenantBankStatementTransaction Parse(RawTransaction rawTransaction)
    {
        var date = Regex.Replace(rawTransaction.Date, "(th|st|nd|rd|d)", string.Empty);

        var delta = string.IsNullOrEmpty(rawTransaction.MoneyIn)
            ? rawTransaction.MoneyOut
            : rawTransaction.MoneyIn;

        var direction = string.IsNullOrEmpty(rawTransaction.MoneyIn)
            ? TransactionDirection.MoneyOut
            : TransactionDirection.MoneyIn;

        return new TenantBankStatementTransaction(
            DateTime.ParseExact(date, "d MMMM yyyy", CultureInfo.InvariantCulture),
            rawTransaction.PaymentType,
            rawTransaction.Details,
            decimal.Parse(delta, NumberStyles.Currency),
            direction,
            decimal.Parse(rawTransaction.Balance, NumberStyles.Currency)
            );
    }
}