using AffordabilityServiceCore.Models;

namespace AffordabilityServiceCore.Abstractions;

public interface IAffordabilityService
{
    IReadOnlyCollection<Property> Check(IReadOnlyCollection<TenantBankStatementTransaction> transactions, 
        IReadOnlyCollection<Property> properties);
}