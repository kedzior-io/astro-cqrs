namespace AstroCqrs;

public interface ICommand : IHandlerMessage
{ }

public interface ICommand<out TResult> : IHandlerMessage<TResult>
{ }