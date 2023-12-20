using System;
using AstroCqrs;
using Handlers.Orders.Commands;

namespace AzureFunctions.Functions;

public class ProcessOrdersTimerTriggerFunction
{
    [Function(nameof(ProcessOrdersTimerTriggerFunction))]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, FunctionContext context)
    {
        await AzureFunctionExtensions.ExecuteTimerAsync<ProcessOrders.Command, ProcessOrders.Response>(context);
    }
}