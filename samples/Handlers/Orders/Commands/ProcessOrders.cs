namespace Handlers.Orders.Commands;

/*
 * An example of a Command without parameters and with response
 */

public static class ProcessOrders
{
    public sealed record Command() : ICommand<IHandlerResponse<Response>>;
    public record Response(string Message);

    public sealed class Handler : CommandHandler<Command, Response>
    {
        public Handler()
        {
        }

        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command command, CancellationToken ct)
        {
            var message = await Task.FromResult("All orders processed succesfully");
            var response = new Response(message);

            return Success(response);
        }
    }
}