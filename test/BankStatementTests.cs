using AffordabilityServiceCore.Models;
using NUnit.Framework;

namespace AffordabilityServiceTests;

public class BankStatementTests
{
    private BankStatement _subject;

    [SetUp]
    public void Setup()
    {
        IReadOnlyCollection<TenantBankStatementTransaction> transactions = 
        [
            new (DateTime.UtcNow.AddMonths(-1), "Test", "Test", 1, TransactionDirection.MoneyIn, 0),
            new (DateTime.UtcNow, "Test", "Test", 1, TransactionDirection.MoneyIn, 0)
        ];

        _subject = new BankStatement(transactions);
    }

    [Test]
    public void BankStatement_WhenGivenValidTransactions_HasMonthlyIncome()
    {
        Assert.That(_subject.MonthlyIncome, Is.EqualTo(1));
    }
}