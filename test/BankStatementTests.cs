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
            new (DateTime.UtcNow.AddMonths(-1), "Income", "Test", 1, TransactionDirection.MoneyIn, 0),
            new (DateTime.UtcNow.AddMonths(-1), "Expense", "Test", 1, TransactionDirection.MoneyOut, 0),
            new (DateTime.UtcNow, "Income", "Test", 1, TransactionDirection.MoneyIn, 0),
            new (DateTime.UtcNow, "Expense", "Test", 1, TransactionDirection.MoneyOut, 0),
        ];

        _subject = new BankStatement(transactions);
    }

    [Test]
    public void BankStatement_WhenGivenValidTransactions_HasMonthlyIncome()
    {
        Assert.That(_subject.MonthlyIncome, Is.EqualTo(1));
    }

    [Test]
    public void BankStatement_WhenGivenValidTransactions_HasMonthlyExpenses()
    {
        Assert.That(_subject.MonthlyExpenses, Is.EqualTo(1));
    }
}