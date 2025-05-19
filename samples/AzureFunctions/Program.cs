using Azure.Core.Serialization;
using Handlers.Abstractions;
using Handlers.Orders.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.WithProperty("SourceLogger", "Serilog");

var logger = loggerConfiguration.CreateLogger();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddSingleton<ILogger>(_ => logger);
        services.AddScoped<IHandlerContext, HandlerContext>();

        // TODO: Looking for a better way to find and register handlers
        services.AddMinimalCqrsFromAssemblyContaining<ListOrders.Query>();

        services.Configure<WorkerOptions>(workerOptions =>
        {
            workerOptions.Serializer = new JsonObjectSerializer(CustomJsonOptions.Defaults);
        });
    })
    .Build();

await host.RunAsync();