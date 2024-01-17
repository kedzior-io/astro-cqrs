using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.BindingModels;

public class ConfirmPaymentModel
{
    [FromQuery]
    public string Id { get; set; } = string.Empty;

    [FromForm]
    public string Reference { get; set; } = string.Empty;
}