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
}