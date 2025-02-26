namespace AffordabilityServiceCore.Models;

public class BankStatement
{
    public IReadOnlyCollection<TenantBankStatementTransaction> Transactions { get; init; }

    public IReadOnlyCollection<DateTime> MonthlyDistinctTransactionTimestamps { get; init; }

    public decimal MonthlyIncome { get; init; }
    public decimal MonthlyExpenses { get; init; }
    public decimal Affordability { get; init; }

    public BankStatement(IReadOnlyCollection<TenantBankStatementTransaction> transactions)
    {
        Transactions = transactions;

        MonthlyDistinctTransactionTimestamps = GetMonthlyDistinctTransactionTimestamps();

        MonthlyIncome = SumMonthlyTransactions(TransactionDirection.MoneyIn);

        MonthlyExpenses = SumMonthlyTransactions(TransactionDirection.MoneyOut);

        Affordability = MonthlyIncome - MonthlyExpenses;
    }

    private IReadOnlyCollection<DateTime> GetMonthlyDistinctTransactionTimestamps()
    {
        return Transactions
            .Select(x => x.TransactionMonth)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }

    private decimal SumMonthlyTransactions(TransactionDirection direction)
    {
        var monthlyTransactions = Transactions
            .Where(x => x.Direction == direction)
            .GroupBy(x => new { x.TransactionType, x.Description })
            .Select(x => new { Transaction = x, Count = x.Count() });

        var monthlyReoccurringTransactions = monthlyTransactions
            .Where(x => x.Count == MonthlyDistinctTransactionTimestamps.Count);

        return monthlyReoccurringTransactions.Sum(x => x.Transaction.First().Delta);
    }
}