using AffordabilityServiceCore.Concretions;
using AffordabilityServiceCore.Models;
using NUnit.Framework;

namespace AffordabilityServiceTests;

public class BankStatementValidatorServiceTests
{
    private BankStatementValidatorService _subject;

    [SetUp]
    public void Setup()
    {
        _subject = new BankStatementValidatorService();
    }

    [Test]
    public void Validate_WhenTransactionsIsEmpty_ReturnsFalse()
    {
        Assert.That(_subject.Validate([]), Is.False);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void Check_WhenInsufficientTransactions_ReturnsFalse(int numberOfTransactions)
    {
        var transactions = new List<TenantBankStatementTransaction>();

        for (var i = 0; i < numberOfTransactions; i++)
        {
            transactions.Add(new TenantBankStatementTransaction(DateTime.UtcNow, "Test", "Test", 0, TransactionDirection.MoneyIn, 0));
        }

        Assert.That(_subject.Validate(transactions), Is.False);
    }
}