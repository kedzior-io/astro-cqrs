using Handlers.Orders.Commands;

namespace AzureFunctions.Functions;

/*
 * An example of reusing a Command in ServiceBus Trigger Azure Function
*/

public class ServiceBusExampleFunction
{
    [Function(nameof(ServiceBusExampleFunction))]
    public async Task Run([ServiceBusTrigger("order-submitted", Connection = "ConnectionStrings:ServiceBus")] string json, FunctionContext context)
    {
        await AzureFunction.ExecuteServiceBusAsync<OrderSubmitted.Command>(json, CustomJsonOptions.Defaults, context);
    }
}