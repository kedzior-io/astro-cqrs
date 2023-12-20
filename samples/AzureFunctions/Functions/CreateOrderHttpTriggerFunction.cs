using AzureFunctions.Configuration;
using Handlers.Orders.Commands;

namespace AstroCqrs.AzureFunctions;

/*
* An example of reusing a Command in HttpTrigger Azure Function
*/

public class CreateOrderHttpTriggerFunction
{
    [Function(nameof(CreateOrderHttpTriggerFunction))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        return await AzureFunctionExtensions.ExecuteHttpPostAsync<CreateOrder.Command, CreateOrder.Response>(req, CustomJsonOptions.Defaults);
    }
}