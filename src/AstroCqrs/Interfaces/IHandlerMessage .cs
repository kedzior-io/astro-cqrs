namespace AstroCqrs;

public interface IHandlerMessage
{ }

public interface IHandlerMessage<out TResponse>
{ }