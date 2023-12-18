using Handlers.Orders.Queries;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AstroCqrs.AzureFunctions;

/*
* An example of reusing a Query in HttpTrigger Azure Function
*/

public class HttpTriggerFunction
{
    private readonly ILogger<HttpTriggerFunction> _logger;

    public HttpTriggerFunction(ILogger<HttpTriggerFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(HttpTriggerFunction))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        return await AzureFunctionExtensions.ExecuteAsync<ListOrders.Query, ListOrders.Response>(req);
    }
}