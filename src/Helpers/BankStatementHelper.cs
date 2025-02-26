using AffordabilityServiceCore.Models;

namespace AffordabilityServiceCore.Helpers;

public static class BankStatementHelper
{
    public static decimal Digest(IReadOnlyCollection<TenantBankStatementTransaction> transactions)
    {
        var distinctOrderedMonthlyTransactionTimestamps = transactions
            .Select(x => x.TransactionMonth)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        var monthlyIncome = transactions
            .Where(x => x.Direction == TransactionDirection.MoneyIn)
            .GroupBy(x => new { x.TransactionType, x.Description })
            .Select(x => new { Transaction = x, Count = x.Count() });

        // Reoccurring income must occur on each month of the statement
        var monthlyReoccurringIncome = monthlyIncome
            .Where(x => x.Count == distinctOrderedMonthlyTransactionTimestamps.Count);

        return monthlyReoccurringIncome.Sum(x => x.Transaction.First().Delta);
    }
}