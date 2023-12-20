using Handlers.Orders.Queries;

namespace AstroCqrs.AzureFunctions;

/*
* An example of reusing a Query in HttpTrigger Azure Function
*/

public class ListOrdersHttpTriggerFunction
{
    [Function(nameof(ListOrdersHttpTriggerFunction))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        return await AzureFunctionExtensions.ExecuteHttpGetAsync<ListOrders.Query, ListOrders.Response>(req, CustomJsonOptions.Defaults);
    }
}