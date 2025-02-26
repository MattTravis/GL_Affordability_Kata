using AffordabilityServiceCore.Concretions;
using AffordabilityServiceCore.Exceptions;
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
}