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
        // Monthly reoccurring income share Type and Description
        var monthlyIncome = transactions
            .Where(x => x.Direction == TransactionDirection.MoneyIn)
            .GroupBy(x => new {x.TransactionType, x.Description})
            .Select(x => new {Key = x.Key, Count = x.Count()});

        var monthlyReoccurringIncome = monthlyIncome
            .Where(x => x.Count == distinctOrderedMonthlyTransactionTimestamps.Count);

        if (!monthlyReoccurringIncome.Any())
        {
            return false;
        }


        return true;
    }
}