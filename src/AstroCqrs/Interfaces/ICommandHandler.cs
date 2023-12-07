namespace AstroCqrs;

public interface IMessageHandler
{ }

public interface IMessageHandler<in TMessage> : IMessageHandler where TMessage : IHandlerMessage
{
    Task ExecuteAsync(TMessage message, CancellationToken ct);
}

public interface IMessageHandler<in TMessage, TResult> : IMessageHandler where TMessage : IHandlerMessage<TResult>
{
    Task<TResult> ExecuteAsync(TMessage message, CancellationToken ct);
}