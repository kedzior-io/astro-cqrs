namespace Handlers.Orders.Commands;

/*
 * An example of a Command without parameters and with response
 */

public static class ProcessOrders
{
    public sealed record Command() : ICommand<Response>;
    public record Response(string Message);

    public sealed class Handler : CommandHandler<Command, Response>
    {
        public Handler()
        {
        }

        public override async Task<Response> ExecuteAsync(Command command, CancellationToken ct)
        {
            var message = await Task.FromResult("All orders processed succesfully");
            return new Response(message);
        }
    }
}