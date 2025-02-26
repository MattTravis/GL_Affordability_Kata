using AffordabilityServiceCore.Helpers;
using AffordabilityServiceCore.Models;
using NUnit.Framework;

namespace AffordabilityServiceTests;

public class BankStatementHelperTests
{
    [Test]
    public void Digest_WhenGivenValidStatement_ReturnsAffordability()
    {
        List<TenantBankStatementTransaction> transactions = 
        [
            new(DateTime.UtcNow.AddMonths(-1), "Test", "Test", 1, TransactionDirection.MoneyIn, 0),
            new(DateTime.UtcNow, "Test", "Test", 1, TransactionDirection.MoneyIn, 0)
        ];

        var affordability = BankStatementHelper.Digest(transactions);

        Assert.That(affordability, Is.EqualTo(1));
    }
}