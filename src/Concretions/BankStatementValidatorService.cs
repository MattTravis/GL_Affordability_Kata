using AffordabilityServiceCore.Abstractions;
using AffordabilityServiceCore.Models;

namespace AffordabilityServiceCore.Concretions;

public class BankStatementValidatorService : IBankStatementValidatorService
{
    public bool Validate(BankStatement statement)
    {
        var distinctOrderedMonthlyTransactionTimestamps = statement.MonthlyDistinctTransactionTimestamps;

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

        // Reoccurring income must greater than zero
        if (statement.MonthlyIncome <= 0)
        {
            return false;
        }

        return true;
    }
}