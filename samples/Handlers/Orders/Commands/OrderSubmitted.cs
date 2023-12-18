namespace Handlers.Orders.Commands;

/*
 * An example of a Command used with Azure Service Bus
 */

public static class OrderSubmitted
{
    public sealed record Command(string Id) : ICommand<Response>;
    public record Response(string Message);

    public sealed class Handler : CommandHandler<Command, Response>
    {
        public Handler()
        {
        }

        public override async Task<Response> ExecuteAsync(Command command, CancellationToken ct)
        {
            var message = await Task.FromResult("Order confirmation email sent");
            return new Response(message);
        }
    }
}