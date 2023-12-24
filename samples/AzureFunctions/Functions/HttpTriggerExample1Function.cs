using Handlers.Orders.Commands;

namespace AstroCqrs.AzureFunctions;

/*
* An example of reusing a Command in HttpTrigger Azure Function
*/

public class HttpTriggerExample1Function
{
    [Function(nameof(HttpTriggerExample1Function))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        return await AzureFunction.ExecuteHttpPostAsync<CreateOrder.Command, CreateOrder.Response>(req, CustomJsonOptions.Defaults);
    }
}