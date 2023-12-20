using Handlers.Orders.Queries;

namespace AstroCqrs.AzureFunctions;

/*
* An example of reusing a Query in HttpTrigger Azure Function
*/

public class GetOrderByIdHttpTriggerFunction
{
    [Function(nameof(GetOrderByIdHttpTriggerFunction))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        return await AzureFunctionExtensions.ExecuteHttpGetAsync<GetOrderById.Query, GetOrderById.Response>(req, CustomJsonOptions.Defaults);
    }
}