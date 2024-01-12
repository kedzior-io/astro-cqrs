namespace Handlers.Emails.Commands;

/*
 * An example of a Command with parameters and no response
 */

public static class SendEmail
{
    public sealed class Command : ICommand<IHandlerResponse>
    {
        public record EmailModel(string TemplateId, string ReceiverEmail, string Subject, object Content);
    }

    public sealed class Handler(IHandlerContext context/*, IEmailProvider emailProvider*/) : CommandHandler<Command>(context)
    {
        public override async Task<IHandlerResponse> ExecuteAsync(Command command, CancellationToken ct)
        {
            await Task.FromResult(Guid.NewGuid());
            // await _emailProvider.Send(command.EmailModel.TemplateId, command.EmailModel.ReceiverEmail, command.EmailModel.Subject, command.EmailModel.Content);
            return Success();
        }
    }
}