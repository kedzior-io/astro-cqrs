using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.BindingModels;

public record CreateOrderTransactionModel(string OrderId, [FromBody] CreateOrderTransactionBodyModel Body);
public record CreateOrderTransactionBodyModel(decimal Amount, string Status);