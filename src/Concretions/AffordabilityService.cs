using AffordabilityServiceCore.Abstractions;
using AffordabilityServiceCore.Exceptions;
using AffordabilityServiceCore.Models;

namespace AffordabilityServiceCore.Concretions;

public class AffordabilityService(IBankStatementValidatorService bankStatementValidatorService): IAffordabilityService
{
    public IReadOnlyCollection<Property> Check(IReadOnlyCollection<TenantBankStatementTransaction> transactions, 
        IReadOnlyCollection<Property> properties)
    {
        var statement = new BankStatement(transactions);

        if (!bankStatementValidatorService.Validate(statement))
        {
            throw new ValidationException("Supply at least two months bank statements.");
        }

        if (properties.Count == 0)
        {
            throw new ValidationException("Supply at least one property to check.");
        }

        return new List<Property>();
    }
}