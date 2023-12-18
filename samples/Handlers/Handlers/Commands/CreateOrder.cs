using FluentValidation;

namespace AstroCqrs.Handlers.Commands;

/*
 * An example of a Command with parameters and response
 */

public static class CreateOrder
{
    public sealed record Command(string CustomerName, decimal Total) : ICommand<Response>;
    public sealed record Response(Guid OrderId);

    public sealed class CommandValidator : Validator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotNull()
                .NotEmpty();
        }
    }

    public sealed class Handler : CommandHandler<Command, Response>
    {
        public Handler()
        {
        }

        public override async Task<Response> ExecuteAsync(Command command, CancellationToken ct)
        {
            var orderId = await Task.FromResult(Guid.NewGuid());
            return new Response(orderId);
        }
    }
}