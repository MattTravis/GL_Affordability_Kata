using AffordabilityServiceCore.Concretions;
using AffordabilityServiceCore.Exceptions;
using AffordabilityServiceCore.Models;
using NUnit.Framework;

namespace AffordabilityServiceTests;

public class AffordabilityServiceTests
{
    private AffordabilityService _subject;

    [SetUp]
    public void Setup()
    {
        _subject = new AffordabilityService();
    }

    [Test]
    public void Check_WhenTransactionsIsEmpty_ThrowsValidationException()
    {
        var properties = new List<Property>
        {
            new(0, "Test", 0)
        };
        Assert.Throws<ValidationException>(() =>_subject.Check(new List<TenantBankStatementTransaction>(), properties));
    }

    [Test]
    public void Check_WhenPropertiesIsEmpty_ThrowsValidationException()
    {
        var transactions = new List<TenantBankStatementTransaction>
        {
            new(DateTime.UtcNow, "Test", "Test", 0, TransactionDirection.MoneyIn, 0)
        };
        Assert.Throws<ValidationException>(() => _subject.Check(transactions, new List<Property>()));
    }
}