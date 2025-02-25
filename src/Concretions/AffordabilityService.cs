using AffordabilityServiceCore.Abstractions;
using AffordabilityServiceCore.Exceptions;
using AffordabilityServiceCore.Models;

namespace AffordabilityServiceCore.Concretions;

public class AffordabilityService: IAffordabilityService
{
    public IReadOnlyCollection<Property> Check(IReadOnlyCollection<TenantBankStatementTransaction> transactions, 
        IReadOnlyCollection<Property> properties)
    {
        if (transactions.Count == 0)
        {
            throw new ValidationException("Supply at least two months bank statements.");
        }

        return new List<Property>();
    }
}