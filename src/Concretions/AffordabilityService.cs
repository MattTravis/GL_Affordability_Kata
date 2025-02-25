using AffordabilityServiceCore.Abstractions;
using AffordabilityServiceCore.Models;

namespace AffordabilityServiceCore.Concretions;

public class AffordabilityService: IAffordabilityService
{
    public IReadOnlyCollection<Property> Check(IReadOnlyCollection<TenantBankStatementTransaction> transactions, 
        IReadOnlyCollection<Property> properties)
    {
        throw new NotImplementedException();
    }
}