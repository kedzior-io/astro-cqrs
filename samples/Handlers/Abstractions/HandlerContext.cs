using Serilog;

namespace Handlers.Abstractions;

public sealed class HandlerContext : IHandlerContext
{
    // public IDbContext DbContext { get; private set; }
    public ILogger Logger { get; private set; }

    public HandlerContext(ILogger logger)
    {
        Logger = logger;
    }
}