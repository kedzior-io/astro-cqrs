using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace AstroCqrs;

public static class WebApplicationExtensions
{
    //public static RouteHandlerBuilder MapGetHandlerV1<TQuery, TResponse>(this WebApplication app, string pattern) where TQuery : IHandlerMessage<TResponse>
    //{
    //    return app.MapGet(pattern, async ([AsParameters] TQuery query, CancellationToken ct) =>
    //    {
    //        return await ExecuteHandlerAsyncV1(query, ct);
    //    });
    //}

    //public static RouteHandlerBuilder MapPostHandlerV1<TCommand, TResponse>(this WebApplication app, string pattern) where TCommand : IHandlerMessage<TResponse>
    //{
    //    return app.MapPost(pattern, async (TCommand? command, CancellationToken ct) =>
    //    {
    //        if (command is not null)
    //        {
    //            return await ExecuteHandlerAsyncV1(command, ct);
    //        }

    //        return Results.Ok(await HandlerExtensions.ExecuteWithEmptyMessageAsync<TCommand, TResponse>(ct));
    //    });
    //}

    //private static async Task<IResult> ExecuteHandlerAsyncV1<TResponse>(IHandlerMessage<TResponse> message, CancellationToken ct)
    //{
    //    var validationResult = await ValidationExtensions.ExecuteValidationAsync(message);

    //    if (validationResult is not null)
    //    {
    //        return Results.ValidationProblem(validationResult, statusCode: (int)HttpStatusCode.BadRequest);
    //    }

    //    return Results.Ok(await HandlerExtensions.ExecuteAsync(message, ct));
    //}

    public static RouteHandlerBuilder MapGetHandler<TQuery, TResponse>(this WebApplication app, string pattern) where TQuery : IHandlerMessage<IHandlerResponse<TResponse>>
    {
        return app.MapGet(pattern, async ([AsParameters] TQuery query, CancellationToken ct) => await ExecuteHandlerAsync(query, ct));
    }

    public static RouteHandlerBuilder MapPostHandler<TCommand, TResponse>(this WebApplication app, string pattern) where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
    {
        return app.MapPost(pattern, async (TCommand? command, CancellationToken ct) =>
        {
            if (command is not null)
            {
                return await ExecuteHandlerAsync(command, ct);
            }

            var response = await HandlerExtensions.ExecuteWithEmptyMessageAsync<TCommand, IHandlerResponse<TResponse>>(ct);

            return CreateResponse(response);
        });
    }

    private static async Task<IResult> ExecuteHandlerAsync<TResponse>(IHandlerMessage<IHandlerResponse<TResponse>> message, CancellationToken ct)
    {
        var validationResult = await ValidationExtensions.ExecuteValidationAsync(message);

        if (validationResult is not null)
        {
            return Results.ValidationProblem(validationResult, statusCode: (int)HttpStatusCode.BadRequest);
        }

        var response = await HandlerExtensions.ExecuteAsync(message, ct);

        return CreateResponse(response);
        /*
        if (response.IsFailure)
        {
            return Results.ValidationProblem(
                title: response.Message,
                errors: new Dictionary<string, string[]>(),
                statusCode: (int)HttpStatusCode.BadRequest
                );
        }

        return Results.Ok(response.Payload);
        */
    }

    private static IResult CreateResponse<TResponse>(IHandlerResponse<TResponse> response)
    {
        if (response.IsFailure)
        {
            return Results.ValidationProblem(
                title: response.Message,
                errors: new Dictionary<string, string[]>(),
                statusCode: (int)HttpStatusCode.BadRequest
            );
        }

        return Results.Ok(response.Payload);
    }
}