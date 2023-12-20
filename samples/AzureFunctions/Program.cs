using AstroCqrs;
using Azure.Core.Serialization;
using Handlers.Orders.Queries;

using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.Serializer = new JsonObjectSerializer(CustomJsonOptions.Defaults);
    })
    .ConfigureServices(services =>
    {
        // TODO: Looking for a better way to find and register handlers
        services.AddAstroCqrsFromAssemblyContaining<ListOrders.Query>();
    })
    .Build();

host.Run();