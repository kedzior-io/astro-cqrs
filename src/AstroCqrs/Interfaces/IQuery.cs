namespace AstroCqrs;

public interface IQuery : IHandlerMessage
{ }

public interface IQuery<out TResult> : IHandlerMessage<TResult>

{ }