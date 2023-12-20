//using Handlers.Orders.Commands;

//namespace AstroCqrs.AzureFunctions;

///*
// * An example of reusing a Command in ServiceBus Trigger Azure Function
//*/

//public class ServiceBusFunction
//{
//    [Function(nameof(ServiceBusFunction))]
//    public async Task Run([ServiceBusTrigger("order-submitted", Connection = "ConnectionStrings:ServiceBus")] string json, FunctionContext context)
//    {
//        await AzureFunctionExtensions.ExecuteServiceBusAsync<OrderSubmitted.Command, OrderSubmitted.Response>(json, CustomJsonOptions.Defaults, context);
//    }
//}