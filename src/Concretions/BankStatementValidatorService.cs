using AffordabilityServiceCore.Abstractions;
using AffordabilityServiceCore.Models;

namespace AffordabilityServiceCore.Concretions;

public class BankStatementValidatorService : IBankStatementValidatorService
{
    public bool Validate(IReadOnlyCollection<TenantBankStatementTransaction> transactions)
    {
        if (transactions.Count < 2)
        {
            return false;
        }

        // Bank statements must cover at least two months

        // Bank statements must cover contiguous months

        // Must have at least one reoccurring income source

        return true;
    }
}