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
        public Handler(IHandlerContext context): base(context)
        {
        }

        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command command, CancellationToken ct)
        {
            var message = await Task.FromResult("All orders processed successfully");
            
            Logger.Information("Some sample log message");

            var response = new Response(message);

            return Success(response);
        }
    }
}