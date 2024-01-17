using AstroCqrs;
using Handlers;
using Handlers.Orders.Commands;
using MinimalApi.BindingModels;

namespace MinimalApi;

/*
 * Custom Minimal API binding.
 * Examples of custom bindings between custom model and a command.
 * This way our handlers are not polluted with AspNetCore's specific attributes such as 'FromBody' and 'FromQuery'
 */

public static class CustomBindingEndpoints
{
    public static WebApplication AddCustomBindingExamples(this WebApplication app)
    {
        // Maps 'ConfirmPaymentModel' (mix of query params + body params) to 'ConfirmPayment.Command'

        app.MapPostHandler<ConfirmPaymentModel, ConfirmPayment.Command>("/orders.payments.confirm.{orderId}",
            (ConfirmPaymentModel model) =>
            {
                return new(model.OrderId, model.Body.Reference);
            }
        );

        // Maps 'CreateOrderTransactionModel' (mix of query params + body params) to 'CreateOrderTransaction.Command' and return a response

        app.MapPostHandler<CreateOrderTransactionModel, CreateOrderTransaction.Command, CreateOrderTransaction.Response>("/orders.transaction.create.{orderId}",
            (CreateOrderTransactionModel model) =>
            {
                return new(model.OrderId, model.Body.Amount, model.Body.Status);
            }
        );

        // Here we map 'ApprovePaymentModel' (mix of query params + form data) to 'ApprovePayment.Command'

        app.MapPostHandler<ApprovePaymentModel, ApprovePayment.Command>("/orders.payments.approve.{orderId}",
            (ApprovePaymentModel model) =>
            {
                return new(model.OrderId, model.SomeFormDataValue);
            }
        ).DisableAntiforgery();

        return app;
    }
}