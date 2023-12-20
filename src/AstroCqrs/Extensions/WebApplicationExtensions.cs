using Azure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace AstroCqrs;

public static class WebApplicationExtensions
{
    public static RouteHandlerBuilder MapGetHandler<TQuery, TResponse>(this WebApplication app, string pattern) where TQuery : IHandlerMessage<TResponse>
    {
        return app.MapGet(pattern, async ([AsParameters] TQuery query, CancellationToken ct) =>
        {
            return await ExecuteHandlerAsync(query, ct);
        });
    }

    public static RouteHandlerBuilder MapPostHandler<TCommand, TResponse>(this WebApplication app, string pattern) where TCommand : IHandlerMessage<TResponse>
    {
        return app.MapPost(pattern, async (TCommand? command, CancellationToken ct) =>
        {
            if (command is not null)
            {
                return await ExecuteHandlerAsync(command, ct);
            }

            return Results.Ok(await HandlerExtensions.ExecuteWithEmptyMessageAsync<TCommand, TResponse>(ct));
        });
    }

    private static async Task<IResult> ExecuteHandlerAsync<TResponse>(IHandlerMessage<TResponse> message, CancellationToken ct)
    {
        var validationResult = await ValidationExtensions.ExecuteValidationAsync(message);

        if (validationResult is not null)
        {
            return Results.ValidationProblem(validationResult, statusCode: (int)HttpStatusCode.BadRequest);
        }

        return Results.Ok(await HandlerExtensions.ExecuteAsync(message, ct));
    }
}