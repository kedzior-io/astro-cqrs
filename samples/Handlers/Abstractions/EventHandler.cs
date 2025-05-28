using Serilog;

namespace Handlers.Abstractions;

public abstract class EventHandler<TEvent> : MinimalCqrs.EventHandler<TEvent> where TEvent : IHandlerMessage
{
    protected readonly ILogger Logger;

    protected EventHandler(IHandlerContext context)
    {
        Logger = context.Logger;
    }
}