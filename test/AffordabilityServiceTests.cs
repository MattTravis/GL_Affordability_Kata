using AffordabilityServiceCore.Concretions;
using AffordabilityServiceCore.Exceptions;
using AffordabilityServiceCore.Models;
using NUnit.Framework;

namespace AffordabilityServiceTests;

public class AffordabilityServiceTests
{
    private AffordabilityService _subject;

    private readonly List<Property> _singleProperty = [new(0, "Test", 0)];
    private readonly List<TenantBankStatementTransaction> _singleTransaction =
        [new(DateTime.UtcNow, "Test", "Test", 0, TransactionDirection.MoneyIn, 0)];

[SetUp]
    public void Setup()
    {
        _subject = new AffordabilityService();
    }

    [Test]
    public void Check_WhenTransactionsIsEmpty_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>_subject.Check(new List<TenantBankStatementTransaction>(), _singleProperty));
    }

    [Test]
    public void Check_WhenPropertiesIsEmpty_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() => _subject.Check(_singleTransaction, new List<Property>()));
    }
}