using AstroCqrs.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;

namespace AstroCqrs;

public static class AzureFunction
{
    public static async Task<HttpResponseData> ExecuteHttpPostAsync<TCommand, TResponse>(HttpRequestData request, JsonSerializerOptions jsonOptions) where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
    {
        var requestBody = await request.BodyAsync();
        IHandlerResponse<TResponse>? response;

        if (string.IsNullOrWhiteSpace(requestBody))
        {
            response = await HandlerExtensions.ExecuteWithEmptyMessageAsync<TCommand, IHandlerResponse<TResponse>>(request.FunctionContext.CancellationToken);
            
            if (response.IsFailure)
            {
                return await Failure(request, response.Message);
            }
            
            return await Success(request, response);
        }

        DeserializeMessage(requestBody, jsonOptions, out TCommand message);

        var validationResult = await ValidationExtensions.ExecuteValidationAsync(message);

        if (validationResult is not null)
        {
            return await Failure(request, validationResult);
        }
        
        response = await HandlerExtensions.ExecuteAsync(message, request.FunctionContext.CancellationToken);

        if (response.IsFailure)
        {
            return await Failure(request, response.Message);
        }
        
        return await Success(request, response.Payload);
    }

    public static async Task<HttpResponseData> ExecuteHttpGetAsync<TQuery, TResponse>(HttpRequestData request, JsonSerializerOptions jsonOptions) where TQuery : IHandlerMessage<IHandlerResponse<TResponse>>
    {
        IHandlerResponse<TResponse>? response;

        if (request.Query.Count == 0)
        {
            response = await HandlerExtensions.ExecuteWithEmptyMessageAsync<TQuery, IHandlerResponse<TResponse>>(request.FunctionContext.CancellationToken);
            
            if (response.IsFailure)
            {
                return await Failure(request, response.Message);
            }
            
            return await Success(request, response);
        }

        DeserializeFromQuery(request.Query, jsonOptions, out TQuery message);

        var validationResult = await ValidationExtensions.ExecuteValidationAsync(message);

        if (validationResult is not null)
        {
            return await Failure(request, validationResult);
        }

        response = await HandlerExtensions.ExecuteAsync(message, request.FunctionContext.CancellationToken);

        if (response.IsFailure)
        {
            return await Failure(request, response.Message);
        }

        return await Success(request, response.Payload);
    }

    public static async Task ExecuteServiceBusAsync<TCommand, TResponse>(string command, JsonSerializerOptions jsonOptions, FunctionContext context) where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            await HandlerExtensions.ExecuteWithEmptyMessageAsync<TCommand, IHandlerResponse<TResponse>>(context.CancellationToken);
        }

        DeserializeMessage(command, jsonOptions, out TCommand message);

        var validationResult = await ValidationExtensions.ExecuteValidationAsync(message);

        if (validationResult is not null)
        {
            FailureWithThrow(message.GetType().FullName!, validationResult);
        }

        await HandlerExtensions.ExecuteAsync(message, context.CancellationToken);
    }

    public static async Task ExecuteTimerAsync<TCommand, TResponse>(FunctionContext context) where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
    {
        await HandlerExtensions.ExecuteWithEmptyMessageAsync<TCommand, IHandlerResponse<TResponse>>(context.CancellationToken);
    }
    private static async Task<HttpResponseData> Success(HttpRequestData request, object? payload)
    {
        var response = request.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(payload);
        return response;
    }

    private static async Task<HttpResponseData> Failure(HttpRequestData request, object? payload)
    {
        var response = request.CreateResponse(HttpStatusCode.BadRequest);
        await response.WriteAsJsonAsync(new
        {
            StatusCode = HttpStatusCode.BadRequest,
            Message = "One or more errors occurred!",
            Errors = payload,
        });
        return response;
    }

    private static void FailureWithThrow(string commandName, object? errors)
    {
        var message = new
        {
            errors
        };

        throw new ArgumentException(commandName, JsonSerializer.Serialize(message));
    }

    private static void DeserializeMessage<TMessage>(string json, JsonSerializerOptions? jsonOptions, out TMessage message)
    {
        message = JsonSerializer.Deserialize<TMessage>(json, jsonOptions)!;
    }

    private static void DeserializeFromQuery<TMessage>(NameValueCollection parameters, JsonSerializerOptions? jsonOptions, out TMessage message)
    {
        var query = parameters.AllKeys.ToDictionary(t => t!, t => parameters[t]!);

        var serializedQuery = JsonSerializer.Serialize(query, jsonOptions);

        DeserializeMessage(serializedQuery, jsonOptions, out message);
    }
}