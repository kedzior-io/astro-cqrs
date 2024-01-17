using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.BindingModels;

public record ApprovePaymentModel(string OrderId, [FromForm] string SomeFormDataValue);