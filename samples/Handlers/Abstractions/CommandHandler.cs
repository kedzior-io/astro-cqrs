using Serilog;

namespace Handlers.Abstractions;

public abstract class CommandHandler<TCommand, TResponse> : Handler<TCommand, TResponse> where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected readonly ILogger Logger;
    // protected readonly IDbContext DbContext;

    protected CommandHandler(IHandlerContext context)
    {
        Logger = context.Logger;
        // DbContext = context.DbContext;
    }
}

public abstract class CommandHandler<TCommand> : Handler<TCommand> where TCommand : IHandlerMessage<IHandlerResponse>
{
    protected readonly ILogger Logger;
    // protected readonly IDbContext DbContext;

    protected CommandHandler(IHandlerContext context)
    {
        Logger = context.Logger;
        // DbContext = context.DbContext;
    }
}