using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.BindingModels;

public record ConfirmPaymentModel(string OrderId, [FromBody] ConfirmPaymentBodyModel Body);
public record ConfirmPaymentBodyModel(string Reference);