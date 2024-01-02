using Serilog;

namespace Handlers.Abstractions;

public abstract class QueryHandler<TQuery, TResponse> : Handler<TQuery, TResponse> where TQuery : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected readonly ILogger Logger;
    // protected readonly IDbContext DbContext;

    protected QueryHandler(IHandlerContext context)
    {
        Logger = context.Logger;
        // DbContext = context.DbContext;
    }
}