using Handlers.Orders.Queries;

namespace AzureFunctions.Functions;

/*
* An example of reusing a Query in HttpTrigger Azure Function
*/

public class HttpTriggerExample3Function
{
    [Function(nameof(HttpTriggerExample3Function))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        return await AzureFunction.ExecuteHttpGetAsync<ListOrders.Query, ListOrders.Response>(req, CustomJsonOptions.Defaults);
    }
}