namespace AffordabilityServiceCore.Models;

public class BankStatement
{
    public IReadOnlyCollection<TenantBankStatementTransaction> Transactions { get; init; }

    public IReadOnlyCollection<DateTime> MonthlyDistinctTransactionTimestamps { get; init; }

    public decimal MonthlyIncome { get; init; }

    public BankStatement(IReadOnlyCollection<TenantBankStatementTransaction> transactions)
    {
        Transactions = transactions;

        MonthlyDistinctTransactionTimestamps = GetMonthlyDistinctTransactionTimestamps();

        MonthlyIncome = GetMonthlyIncome();
    }

    private IReadOnlyCollection<DateTime> GetMonthlyDistinctTransactionTimestamps()
    {
        return Transactions
            .Select(x => x.TransactionMonth)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }

    private decimal GetMonthlyIncome()
    {
        var monthlyIncome = Transactions
            .Where(x => x.Direction == TransactionDirection.MoneyIn)
            .GroupBy(x => new { x.TransactionType, x.Description })
            .Select(x => new { Transaction = x, Count = x.Count() });

        // Reoccurring income must occur on each month of the statement
        var monthlyReoccurringIncome = monthlyIncome
            .Where(x => x.Count == MonthlyDistinctTransactionTimestamps.Count);

        return monthlyReoccurringIncome.Sum(x => x.Transaction.First().Delta);
    }
}