﻿using AffordabilityServiceCore.Abstractions;
using AffordabilityServiceCore.Concretions;
using AffordabilityServiceCore.Exceptions;
using AffordabilityServiceCore.Models;
using Moq;
using NUnit.Framework;

namespace AffordabilityServiceTests;

public class AffordabilityServiceTests
{
    private AffordabilityService _subject;

    private readonly List<Property> _singleProperty = [new(0, "Test", 0)];
    private readonly List<TenantBankStatementTransaction> _singleTransaction =
        [new(DateTime.UtcNow, "Test", "Test", 0, TransactionDirection.MoneyIn, 0)];

    private readonly Mock<IBankStatementValidatorService> _bankStatementValidatorMock = new();

    [SetUp]
    public void Setup()
    {
        _bankStatementValidatorMock.Setup(s => s.Validate(It.IsAny<BankStatement>()))
            .Returns(true);

        _subject = new AffordabilityService(_bankStatementValidatorMock.Object);
    }

    [Test]
    public void Check_WhenTransactionsValidationFails_ThrowsValidationException()
    {
        _bankStatementValidatorMock.Setup(s => s.Validate(It.IsAny<BankStatement>()))
            .Returns(false);
        Assert.Throws<ValidationException>(() =>_subject.Check(new List<TenantBankStatementTransaction>(), _singleProperty));
    }

    [Test]
    public void Check_WhenPropertiesIsEmpty_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() => _subject.Check(_singleTransaction, new List<Property>()));
    }

    [Test] public void Check_WhenAffordable_ReturnsAffordableProperties()
    {
        List<TenantBankStatementTransaction> transactions =
        [
            new(DateTime.UtcNow.AddMonths(-1), "Income", "Test", 1.26m, TransactionDirection.MoneyIn, 0),
            new(DateTime.UtcNow, "Income", "Test", 1.26m, TransactionDirection.MoneyIn, 0),
        ];
        List<Property> properties = 
        [
            new (0, "Test", 1)
        ];
        
        var response = _subject.Check(transactions, properties);

        Assert.That(properties.All(x => response.Any(y => y.Id == x.Id)));
    }
}