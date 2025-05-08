using Handlers.Emails.Commands;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;

namespace AzureFunctions.Functions;

/*
* An example of Azure Function + OpenApi
* http://localhost:7280/api/swagger/ui#/
*/

public sealed record SendEmailCommand : SendEmail.Command;

public class HttpTriggerOpenApiExample1Function
{
    [Function(nameof(HttpTriggerOpenApiExample1Function))]
    [OpenApiOperation(operationId: "Run")]
    [OpenApiRequestBody("application/json", typeof(SendEmailCommand))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        return await AzureFunction.ExecuteHttpPostAsync<SendEmail.Command>(req, CustomJsonOptions.Defaults);
    }
}