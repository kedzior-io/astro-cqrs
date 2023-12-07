using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AstroCqrs;

public static class WebApplicationExtensions
{
    public static RouteHandlerBuilder MapGetHandler<TQuery, TResponse>(this WebApplication app, string pattern) where TQuery : IHandlerMessage<TResponse>
    {
        return app.MapGet(pattern, async ([AsParameters] TQuery query, CancellationToken ct) => await HandlerExtensions.ExecuteAsync(query, ct));
    }

    public static RouteHandlerBuilder MapPostHandler<TCommand, TResponse>(this WebApplication app, string pattern) where TCommand : IHandlerMessage<TResponse>
    {
        return app.MapPost(pattern, async (TCommand? command, CancellationToken ct) =>
        {
            if (command is not null)
            {
                return await HandlerExtensions.ExecuteAsync(command, ct);
            }

            return await HandlerExtensions.ExecuteGenericAsync<TCommand, TResponse>(ct);
        });
    }
}