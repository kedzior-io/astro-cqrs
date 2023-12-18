using Handlers.Orders.Commands;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AstroCqrs.AzureFunctions;

/*
 * TODO: Untested
 * An example of reusing a Command in ServiceBus Trigger Azure Function
*/

public class ServiceBusFunction
{
    private readonly ILogger<ServiceBusFunction> _logger;

    public ServiceBusFunction(ILogger<ServiceBusFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ServiceBusFunction))]
    public async Task Run([ServiceBusTrigger("order-submitted", Connection = "ConnectionStrings:ServiceBus")] string json, FunctionContext context)
    {
        await AzureFunctionExtensions.ExecuteAsync<OrderSubmitted.Command, OrderSubmitted.Response>(json, JsonOptions.Defaults, context);
    }
}