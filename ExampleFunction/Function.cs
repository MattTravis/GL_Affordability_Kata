using AffordabilityServiceCore.Abstractions;
using AffordabilityServiceCore.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Azure.Functions.Worker;
using System.Globalization;
using ValidationException = AffordabilityServiceCore.Exceptions.ValidationException;

namespace ExampleFunction;

public class Function(IAffordabilityService affordabilityService)
{
    [Function("AffordabilityFunction")]
    public async Task<IStatusCodeActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
        CancellationToken ct)
    {
        if (req.Form.Files.Count != 2)
        {
            return new BadRequestObjectResult(new
            {
                Error = "Missing form files.",
                Message = "Upload a bank statement and property list as CSV format."
            });
        }

        var propertiesFormFile = req.Form.Files.SingleOrDefault(x => x.FileName == "properties.csv");
        var bankStatementFormFile = req.Form.Files.SingleOrDefault(x => x.FileName == "bank_statement.csv");

        if (propertiesFormFile is null || bankStatementFormFile is null)
        {
            return new BadRequestObjectResult(new
            {
                Error = "Missing form files.",
                Message = "Upload a bank statement and property list as CSV format, with the correct filenames in the request body."
            });
        }
        
        var transactions = await ProcessBankStatementFile(bankStatementFormFile);
        
        var properties = await ProcessPropertiesFile(propertiesFormFile);

        try
        {
            var result = affordabilityService.Check(transactions, properties);
            return new OkObjectResult(result);
        }
        catch (ValidationException e)
        {
            return new BadRequestObjectResult(new{Error = e.GetType().Name, e.Message});
        }
    }

    private static async Task<List<Property>> ProcessPropertiesFile(IFormFile propertiesFormFile)
    {
        using var propertiesStreamReader = new StreamReader(propertiesFormFile.OpenReadStream());
        using var propertiesCsvReader = new CsvReader(propertiesStreamReader, CultureInfo.InvariantCulture);
        propertiesCsvReader.Context.RegisterClassMap<PropertyMap>();

        await propertiesCsvReader.ReadAsync();
        propertiesCsvReader.ReadHeader();
        var properties = propertiesCsvReader
            .GetRecords<Property>()
            .ToList();
        return properties;
    }

    private static async Task<List<TenantBankStatementTransaction>> ProcessBankStatementFile(IFormFile bankStatementFormFile)
    {
        using var bankStatementStreamReader = new StreamReader(bankStatementFormFile.OpenReadStream());
        using var bankStatementCsvReader = new CsvReader(bankStatementStreamReader, CultureInfo.InvariantCulture);
        bankStatementCsvReader.Context.RegisterClassMap<RawTransactionMap>();

        for (var i = 0; i < 10; i++)
        {
            await bankStatementCsvReader.ReadAsync();
        }

        bankStatementCsvReader.ReadHeader();
        await bankStatementCsvReader.ReadAsync();

        var transactions = bankStatementCsvReader
            .GetRecords<RawTransaction>()
            .Select(TenantBankStatementTransaction.Parse)
            .ToList();

        return transactions;
    }
}

public sealed class PropertyMap : ClassMap<Property>
{
    public PropertyMap()
    {
        Map(x => x.Id).Name("Id");
        Map(x => x.Address).Name("Address");
        Map(x => x.MonthlyRent).Name("Price (pcm)");
    }
}

public sealed class RawTransactionMap : ClassMap<RawTransaction>
{
    public RawTransactionMap()
    {
        Map(x => x.Date).Name("Date");
        Map(x => x.PaymentType).Name("Payment Type");
        Map(x => x.Details).Name("Details");
        Map(x => x.MoneyIn).Name("Money In");
        Map(x => x.MoneyOut).Name("Money Out");
        Map(x => x.Balance).Name("Balance");
    }
}