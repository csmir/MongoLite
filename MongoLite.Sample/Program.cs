using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoLite;
using MongoLite.Sample;

await Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<MongoHost>();
        services.AddHostedService<ExampleModelService>();
    })
    .RunConsoleAsync();