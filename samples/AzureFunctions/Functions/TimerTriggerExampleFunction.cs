using Handlers.Orders.Commands;

namespace AzureFunctions.Functions;

public class TimerTriggerExampleFunction
{
    [Function(nameof(TimerTriggerExampleFunction))]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, FunctionContext context)
    {
        await AzureFunction.ExecuteTimerAsync<ProcessOrders.Command, ProcessOrders.Response>(context);
    }
}