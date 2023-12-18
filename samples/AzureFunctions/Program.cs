using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AstroCqrs;
using Handlers.Orders.Queries;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        // TODO: Any way to avoid that?
        services.AddAstroCqrsFromAssemblyContaining<ListOrders.Query>();
    })
    .Build();

host.Run();