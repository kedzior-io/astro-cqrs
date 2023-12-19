using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace AstroCqrs;

public static class AzureFunctionExtensions
{
    public static async Task ExecuteAsync<TCommand, TResponse>(string deserializedCommand, JsonSerializerOptions jsonOptions, FunctionContext context) where TCommand : IHandlerMessage<TResponse>
    {
        DeserializeCommand(deserializedCommand, jsonOptions, out TCommand command);

        // TODO: Not linking this very much
        await ValidationExtensions.ExecuteValidationAsync(command);

        await HandlerExtensions.ExecuteAsync(command, context.CancellationToken);
    }

    public static async Task<HttpResponseData> ExecuteAsync<TQuery, TResponse>(HttpRequestData request) where TQuery : IHandlerMessage<TResponse>
    {
        // TODO: Not linking this very much
        await ValidationExtensions.ExecuteValidation<TQuery, TResponse>();

        var payload = await HandlerExtensions.ExecuteGenericAsync<TQuery, TResponse>(request.FunctionContext.CancellationToken);
        var response = request.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(payload);
        return response;
    }

    private static void DeserializeCommand<TMessage>(string json, JsonSerializerOptions jsonOptions, out TMessage message)
    {
        message = JsonSerializer.Deserialize<TMessage>(json, jsonOptions)!;
    }
}