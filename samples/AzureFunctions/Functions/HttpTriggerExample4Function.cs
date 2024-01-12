using Handlers.Emails.Commands;

namespace AzureFunctions.Functions;

/*
* An example of reusing a Query in HttpTrigger Azure Function
*/

public class HttpTriggerExample4Function
{
    [Function(nameof(HttpTriggerExample4Function))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        return await AzureFunction.ExecuteHttpPostAsync<SendEmail.Command>(req, CustomJsonOptions.Defaults);
    }
}