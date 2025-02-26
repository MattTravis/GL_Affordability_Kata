using AffordabilityServiceCore.Abstractions;
using AffordabilityServiceCore.Models;

namespace AffordabilityServiceCore.Concretions;

public class BankStatementValidatorService : IBankStatementValidatorService
{
    public bool Validate(IReadOnlyCollection<TenantBankStatementTransaction> transactions)
    {
        var distinctOrderedMonthlyTransactionTimestamps = transactions
            .Select(x => x.TransactionMonth)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        // Bank statements must cover at least two months, minimum valid transaction count is 2.
        if (distinctOrderedMonthlyTransactionTimestamps.Count < 2)
        {
            return false;
        }

        // Bank statements must cover contiguous months
        var previousTimestamp = distinctOrderedMonthlyTransactionTimestamps.Min(x => x.AddMonths(-1));
        foreach (var timestamp in distinctOrderedMonthlyTransactionTimestamps)
        {
            if (previousTimestamp.AddMonths(1) == timestamp)
            {
                previousTimestamp = timestamp;
                continue;
            }

            return false;
        }

        // Must have at least one income source
        if (transactions.All(x => x.Direction == TransactionDirection.MoneyOut))
        {
            return false;
        }

        var atLeastOnIncomeAMonth = distinctOrderedMonthlyTransactionTimestamps
            .All(timestamp =>
                transactions.Any(x => x.TransactionMonth == timestamp && x.Direction == TransactionDirection.MoneyIn));

        if (!atLeastOnIncomeAMonth)
        {
            return false;
        }

        return true;
    }
}