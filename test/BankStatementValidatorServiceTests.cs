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
        Assert.That(_subject.Validate(new BankStatement([])), Is.False);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void Validate_WhenInsufficientTransactions_ReturnsFalse(int numberOfTransactions)
    {
        var transactions = new List<TenantBankStatementTransaction>();

        for (var i = 0; i < numberOfTransactions; i++)
        {
            transactions.Add(new TenantBankStatementTransaction(DateTime.UtcNow, "Test", "Test", 0, TransactionDirection.MoneyIn, 0));
        }

        Assert.That(_subject.Validate(new BankStatement(transactions)), Is.False);
    }

    [Test]
    public void Validate_WhenIncongruentMonths_ReturnsFalse()
    {
        List<TenantBankStatementTransaction> transactions = 
        [
            new(DateTime.UtcNow.AddMonths(-2), "Test", "Test", 0, TransactionDirection.MoneyIn, 0),
            new(DateTime.UtcNow, "Test", "Test", 0, TransactionDirection.MoneyIn, 0)
        ];

        Assert.That(_subject.Validate(new BankStatement(transactions)), Is.False);
    }

    [Test]
    public void Validate_WhenNoReoccurringIncome_NoMoneyIn_ReturnsFalse()
    {
        List<TenantBankStatementTransaction> transactions =
        [
            new(DateTime.UtcNow.AddMonths(-1), "Test", "Test", 0, TransactionDirection.MoneyOut, 0),
            new(DateTime.UtcNow, "Test", "Test", 0, TransactionDirection.MoneyOut, 0)
        ];

        Assert.That(_subject.Validate(new BankStatement(transactions)), Is.False);
    }

    [Test]
    public void Validate_WhenNoReoccurringIncome_NoMonthlyMoneyIn_ReturnsFalse()
    {
        List<TenantBankStatementTransaction> transactions =
        [
            new(DateTime.UtcNow.AddMonths(-1), "Test", "Test", 0, TransactionDirection.MoneyIn, 0),
            new(DateTime.UtcNow, "Test", "Test", 0, TransactionDirection.MoneyOut, 0)
        ];

        Assert.That(_subject.Validate(new BankStatement(transactions)), Is.False);
    }

    [Test]
    public void Validate_WhenNoReoccurringIncome_NoReoccurringMoneyIn_ReturnsFalse()
    {
        List<TenantBankStatementTransaction> transactions =
        [
            new(DateTime.UtcNow.AddMonths(-1), "Test", "Test", 0, TransactionDirection.MoneyIn, 0),
            new(DateTime.UtcNow, "Test2", "Test2", 1, TransactionDirection.MoneyIn, 0)
        ];

        Assert.That(_subject.Validate(new BankStatement(transactions)), Is.False);
    }

    [Test]
    public void Validate_WhenNoReoccurringIncome_ReoccurringMoneyInLessThanOrEqualToZero_ReturnsFalse()
    {
        List<TenantBankStatementTransaction> transactions =
        [
            new(DateTime.UtcNow.AddMonths(-1), "Test", "Test", 0, TransactionDirection.MoneyIn, 0),
            new(DateTime.UtcNow, "Test", "Test", 0, TransactionDirection.MoneyIn, 0)
        ];

        Assert.That(_subject.Validate(new BankStatement(transactions)), Is.False);
    }

    [Test]
    public void Validate_WhenReoccurringIncome_ReturnsTrue()
    {
        List<TenantBankStatementTransaction> transactions =
        [
            new(DateTime.UtcNow.AddMonths(-1), "Test", "Test", 1, TransactionDirection.MoneyIn, 0),
            new(DateTime.UtcNow, "Test", "Test", 1, TransactionDirection.MoneyIn, 0)
        ];

        Assert.That(_subject.Validate(new BankStatement(transactions)), Is.True);
    }
}