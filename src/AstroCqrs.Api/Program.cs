using AstroCqrs;

using AstroCqrs.Handlers.Queries;
using AstroCqrs.Handlers.Commands;

/*
  TODO:

  1. [Done] Command Handler
  2. Command Validation
  3. [Done] Abstract Minimal Api
  4. [Done] Azure Functions - HTTP Request
  5. [Done - Untested] Azure Functions - Service Bus
  6. Inject DbContext
  7. Handler Context
  8. Request Context

 */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAstroCqrs();

builder.Services.AddAuthentication();
builder.Services.AddAuthentication();

var app = builder.Build();

app.MapGetHandler<ListOrders.Query, ListOrders.Response>("/orders.list");

app.MapGetHandler<GetOrderById.Query, GetOrderById.Response>("/orders.getById.{id}");

app.MapPostHandler<CreateOrder.Command, CreateOrder.Response>("/orders.create");

app.MapPostHandler<ProcessOrders.Command, ProcessOrders.Response>("/orders.process");
//.RequireAuthorization();

app.Run();