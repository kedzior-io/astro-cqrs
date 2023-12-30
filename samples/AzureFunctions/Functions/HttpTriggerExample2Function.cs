using Handlers.Orders.Queries;

namespace AzureFunctions.Functions;

/*
* An example of reusing a Query in HttpTrigger Azure Function
*/

public class HttpTriggerExample2Function
{
    [Function(nameof(HttpTriggerExample2Function))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        return await AzureFunction.ExecuteHttpGetAsync<GetOrderById.Query, GetOrderById.Response>(req, CustomJsonOptions.Defaults);
    }
}