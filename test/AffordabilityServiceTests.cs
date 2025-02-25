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
        Assert.Throws<ValidationException>(() =>_subject.Check(new List<TenantBankStatementTransaction>(), new List<Property>()));
    }
}