namespace Handlers.Orders.Events;

public static class OrderCreated
{
    public sealed record Event(string CustomerName) : IEvent;

    public sealed class Handler : Abstractions.EventHandler<Event>
    {
        public Handler(IHandlerContext context) : base(context)
        {
        }

        public override async Task ExecuteAsync(Event @event, CancellationToken ct = default)
        {
            await Task.FromResult($"OrderCreated for {@event.CustomerName}");
        }
    }
}