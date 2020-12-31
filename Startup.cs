using System;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(Fmg.Startup))]
namespace Fmg
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var cosmosConnection = Environment.GetEnvironmentVariable("Cosmos:Connection") ?? string.Empty;

            builder.Services
                .AddLogging(options => options.AddFilter("Fmg", LogLevel.Information))
                .AddSingleton(provider =>
                {
                    return new CosmosClientBuilder(cosmosConnection)
                        .WithBulkExecution(true)
                        .WithConnectionModeGateway()
                        .Build();
                });
        }
    }
}