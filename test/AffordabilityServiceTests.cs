using AffordabilityServiceCore.Abstractions;
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

    [Test] 
    public void Check_WhenAffordable_ReturnsAffordableProperties()
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

    [Test]
    public void Check_WhenUnaffordable_ReturnsNoProperties()
    {
        List<TenantBankStatementTransaction> transactions =
        [
            new(DateTime.UtcNow.AddMonths(-1), "Income", "Test", 1.25m, TransactionDirection.MoneyIn, 0),
            new(DateTime.UtcNow, "Income", "Test", 1.25m, TransactionDirection.MoneyIn, 0),
        ];
        List<Property> properties =
        [
            new (0, "Test", 1)
        ];

        var response = _subject.Check(transactions, properties);

        Assert.That(response.Count == 0);
    }

    // Task 1
    [Test]
    public void Check_WhenSomeAffordable_SomeUnaffordable_ReturnsOnlyAffordableProperties()
    {
        //"1st January 2020", "Direct Debit", "Gas & Electricity", "£95.06", "", "£1200.04"
        //"2nd January 2020", "ATM", "HSBC Holborn", "£20.00", "", "£1180.04"
        //"3rd January 2020", "Standing Order", "London Room", "£500.00", "", "£680.04"
        //"4th January 2020", "Bank Credit", "Awesome Job Ltd", "", "£1254.23", "£1934.27"
        //"1st February 2020", "Direct Debit", "Gas & Electricity", "£95.06", "", "£1839.21"
        //"2nd February 2020", "ATM", "@Random", "£50.00", "", "£1789.21"
        //"3rd February 2020", "Standing Order", "London Room", "£500.00", "", "£1289.21"
        //"4th February 2020", "Bank Credit", "Awesome Job Ltd", "", "£1254.23", "£2543.44"

        List<TenantBankStatementTransaction> transactions =
        [
            new(new DateTime(2020, 1, 1), "Direct Debit", "Gas & Electricity", 95.06m, TransactionDirection.MoneyOut, 1200.04m),
            new(new DateTime(2020, 1, 2), "ATM", "HSBC Holborn", 20.00m, TransactionDirection.MoneyOut, 1180.04m),
            new(new DateTime(2020, 1, 3), "Standing Order", "London Room", 500.00m, TransactionDirection.MoneyOut, 680.04m),
            new(new DateTime(2020, 1, 4), "Bank Credit", "Awesome Job Ltd", 1254.23m, TransactionDirection.MoneyIn, 1934.27m),
            new(new DateTime(2020, 2, 1), "Direct Debit", "Gas & Electricity", 95.06m, TransactionDirection.MoneyOut, 1839.21m),
            new(new DateTime(2020, 2, 2), "ATM", "@Random", 50.00m, TransactionDirection.MoneyOut, 1789.21m),
            new(new DateTime(2020, 2, 3), "Standing Order", "London Room", 500.00m, TransactionDirection.MoneyOut, 1289.21m),
            new(new DateTime(2020, 2, 4), "Bank Credit", "Awesome Job Ltd", 1254.23m, TransactionDirection.MoneyIn, 2543.44m),
        ];

        //1, "1, Oxford Street", 300
        //2, "12, St John Avenue", 750
        //3, "Flat 43, Expensive Block", 1200
        //4, "Flat 44, Expensive Block", 1150
        List<Property> properties =
        [
            new (1, "1, Oxford Street", 300),
            new (2, "12, St John Avenue", 750),
            new (3, "Flat 43, Expensive Block", 1200),
            new (4, "Flat 44, Expensive Block", 1150),
        ];

        var response = _subject.Check(transactions, properties);

        Assert.That(response, Has.Count.EqualTo(1));
        Assert.That(response.First().Address, Is.EqualTo("1, Oxford Street"));
    }
}