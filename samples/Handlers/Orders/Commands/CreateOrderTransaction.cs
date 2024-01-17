using FluentValidation;

namespace Handlers.Orders.Commands;

/*
 * An example of a Command with parameters mapped from both querty string {orderId} and body {amount} and response
 */

public static class CreateOrderTransaction
{
    public sealed record Command(string OrderId, decimal Amount, string Status) : ICommand<IHandlerResponse<Response>>;

    public sealed record Response(string TransactionId);

    public sealed class OrderSubmittedValidator : Validator<Command>
    {
        public OrderSubmittedValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty();

            RuleFor(x => x.Amount)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.Status)
                .NotEmpty();
        }
    }

    public sealed class Handler(IHandlerContext context) : CommandHandler<Command, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command command, CancellationToken ct)
        {
            await Task.FromResult("Order payment confirmed");

            return Success(new Response($"{command.OrderId}+{command.Amount}+{command.Status}"));
        }
    }
}