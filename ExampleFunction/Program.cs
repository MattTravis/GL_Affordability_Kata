using AffordabilityServiceCore.Abstractions;
using AffordabilityServiceCore.Concretions;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddSingleton<IAffordabilityService, AffordabilityService>();
builder.Services.AddSingleton<IBankStatementValidatorService, BankStatementValidatorService>();

builder.Build().Run();
