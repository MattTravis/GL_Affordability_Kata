using AffordabilityServiceCore.Abstractions;
using AffordabilityServiceCore.Models;

namespace AffordabilityServiceCore.Concretions;

public class BankStatementValidatorService : IBankStatementValidatorService
{
    public bool Validate(IReadOnlyCollection<TenantBankStatementTransaction> transactions)
    {
        var distinctOrderedMonthlyTransactionTimestamps = transactions
            .Select(x => new DateTime(x.Timestamp.Year, x.Timestamp.Month, 1))
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        // Bank statements must cover at least two months, minimum valid transaction count is 2.
        if (distinctOrderedMonthlyTransactionTimestamps.Count < 2)
        {
            return false;
        }

        // Bank statements must cover contiguous months
        

        // Must have at least one reoccurring income source

        return true;
    }
}