using FluentValidation;

namespace Handlers.Orders.Commands;

/*
 * An example of a Command used with Azure Service Bus
 */

public static class OrderSubmitted
{
    public sealed record Command(string Id) : ICommand<IHandlerResponse>;

    public sealed class OrderSubmittedValidator : Validator<Command>
    {
        public OrderSubmittedValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    public sealed class Handler(IHandlerContext context) : CommandHandler<Command>(context)
    {
        public override async Task<IHandlerResponse> ExecuteAsync(Command command, CancellationToken ct)
        {
            await Task.FromResult("Order confirmation email sent");

            return Success();
        }
    }
}