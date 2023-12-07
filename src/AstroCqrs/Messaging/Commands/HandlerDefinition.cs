namespace AstroCqrs;

internal sealed class HandlerDefinition
{
    internal Type HandlerType { get; set; }
    internal object? HandlerExecutor { get; set; }

    internal HandlerDefinition(Type handlerType)
    {
        HandlerType = handlerType;
    }
}