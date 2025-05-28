using System.Collections.Concurrent;

namespace MinimalCqrs;

public static class EventBus
{
    private static readonly ConcurrentDictionary<Type, Func<IEvent, CancellationToken, Task>> _events = new();

    public static Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default) where TEvent : IEvent
    {
        var messageType = @event.GetType();

        var registry = Conf.ServiceResolver.Resolve<HandlerRegistry>();

        if (!registry.TryGetValue(messageType, out var handlerDefinition))
        {
            throw new InvalidOperationException($"Unable to create an instance of the handler for [{messageType.FullName}]");
        }

        handlerDefinition.HandlerExecutor ??= CreateHandlerExecutor(messageType);

        return ((HandlerExecutorBase)handlerDefinition.HandlerExecutor).Execute(@event, handlerDefinition.HandlerType, ct);
    }

    private static HandlerExecutorBase CreateHandlerExecutor(Type tEvent)
    => (HandlerExecutorBase)
        Conf.ServiceResolver.CreateSingleton(Types.CommandHandlerExecutorOf1.MakeGenericType(tEvent));
}