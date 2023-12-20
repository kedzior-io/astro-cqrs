using AstroCqrs;

using Handlers.Orders.Commands;

using Handlers.Orders.Queries;

/*
  TODO:

  1. [Done] Command Handler
  2. [Done] Command Validation
  3. [Done] Abstract Minimal Api
  4. [Done] Azure Functions - HTTP Request
  5. [Done] Azure Functions - Service Bus
  6. Inject DbContext
  7. Handler Context
  8. Request Context
  9. Authorization example //.RequireAuthorization();
  10. Handler Response

 */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAstroCqrs();

var app = builder.Build();

app.MapGetHandler<ListOrders.Query, ListOrders.Response>("/orders.list");

app.MapGetHandler<GetOrderById.Query, GetOrderById.Response>("/orders.getById.{id}");

app.MapGetHandler<GetOrderByTotal.Query, GetOrderByTotal.Response>("/orders.getByTotal.{totalValue}");

app.MapPostHandler<CreateOrder.Command, CreateOrder.Response>("/orders.create");

app.MapPostHandler<ProcessOrders.Command, ProcessOrders.Response>("/orders.process");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();