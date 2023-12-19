namespace AstroCqrs;

public interface ICommand : IHandlerMessage
{ }

public interface ICommand<out TResponse> : IHandlerMessage<TResponse>
{ }