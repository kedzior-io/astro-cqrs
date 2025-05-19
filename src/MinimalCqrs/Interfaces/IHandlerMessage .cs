namespace MinimalCqrs;

public interface IHandlerMessage
{ }

public interface IHandlerMessage<out TResponse>
{ }