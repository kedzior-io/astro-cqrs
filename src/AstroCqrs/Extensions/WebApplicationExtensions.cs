using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace AstroCqrs;

public static class WebApplicationExtensions
{
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

    public static RouteHandlerBuilder MapPostHandler<TCommand>(this WebApplication app, string pattern) where TCommand : IHandlerMessage<IHandlerResponse>
    {
        return app.MapPost(pattern, async (TCommand? command, CancellationToken ct) =>
        {
            if (command is not null)
            {
                return await ExecuteHandlerAsync(command, ct);
            }

            var response = await HandlerExtensions.ExecuteWithEmptyMessageAsync<TCommand, IHandlerResponse>(ct);

            return CreateNoContentResponse(response);
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
    }

    private static async Task<IResult> ExecuteHandlerAsync(IHandlerMessage<IHandlerResponse> message, CancellationToken ct)
    {
        var validationResult = await ValidationExtensions.ExecuteValidationAsync(message);

        if (validationResult is not null)
        {
            return Results.ValidationProblem(validationResult, statusCode: (int)HttpStatusCode.BadRequest);
        }

        var response = await HandlerExtensions.ExecuteAsync(message, ct);

        return CreateNoContentResponse(response);
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

    private static IResult CreateNoContentResponse(IHandlerResponse response)
    {
        if (response.IsFailure)
        {
            return Results.ValidationProblem(
                title: response.Message,
                errors: new Dictionary<string, string[]>(),
                statusCode: (int)HttpStatusCode.BadRequest
            );
        }

        return Results.NoContent();
    }
}