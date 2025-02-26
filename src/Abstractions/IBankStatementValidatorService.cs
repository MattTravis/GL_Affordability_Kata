using AffordabilityServiceCore.Models;

namespace AffordabilityServiceCore.Abstractions;

public interface IBankStatementValidatorService
{
    bool Validate(BankStatement statement);
}