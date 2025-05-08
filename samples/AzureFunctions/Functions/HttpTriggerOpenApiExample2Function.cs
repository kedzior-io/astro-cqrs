using Handlers.Emails.Commands;
using Handlers.Orders.Commands;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Net;

namespace AzureFunctions.Functions;

/*
* An example of Azure Function + OpenApi
* http://localhost:7280/api/swagger/ui#/
*/

public sealed record CreateOrderCommand : CreateOrder.Command
{
    public CreateOrderCommand(string customerName, decimal total)
        : base(customerName, total)
    {
    }
}

public sealed record CreateOrderResponse : CreateOrder.Response
{
    public CreateOrderResponse(Guid OrderId, string SomeValue)
        : base(OrderId, SomeValue)
    {
    }
}

public class HttpTriggerOpenApiExample2Function
{
    [Function(nameof(HttpTriggerOpenApiExample2Function))]
    [OpenApiOperation(operationId: "Run")]
    [OpenApiRequestBody("application/json", typeof(CreateOrderCommand))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateOrderResponse))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        return await AzureFunction.ExecuteHttpPostAsync<CreateOrder.Command>(req, CustomJsonOptions.Defaults);
    }
}