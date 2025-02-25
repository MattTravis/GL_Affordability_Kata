namespace AffordabilityServiceCore.Models;

public record TenantBankStatementTransaction(DateTime Timestamp, string TransactionType, string Description, decimal Delta, TransactionDirection Direction, decimal Balance);