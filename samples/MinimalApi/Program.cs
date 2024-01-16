using AstroCqrs;
using Handlers.Abstractions;
using Handlers.Emails.Commands;
using Handlers.Orders.Commands;
using Handlers.Orders.Queries;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration
        .WriteTo.Console();
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
});

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddCors();
builder.Services.AddScoped<IHandlerContext, HandlerContext>();

builder.Services.AddAstroCqrs();

var app = builder.Build();

app.MapGetHandler<ListOrders.Query, ListOrders.Response>("/orders.list");

app.MapGetHandler<GetOrderById.Query, GetOrderById.Response>("/orders.getById.{id}");

app.MapGetHandler<GetOrderByTotal.Query, GetOrderByTotal.Response>("/orders.getByTotal.{totalValue}");

app.MapGetHandler<GetOrderByCustomerId.Query, GetOrderByCustomerId.Response>("/orders.getOrderByCustomerId.{id}");

app.MapPostHandler<CreateOrder.Command, CreateOrder.Response>("/orders.create");

app.MapPostHandler<ProcessOrders.Command, ProcessOrders.Response>("/orders.process");

app.MapPostHandler<SendEmail.Command>("/send.email");

app.MapPostHandler<GetOrderAuthorized.Query, GetOrderAuthorized.Response>("/orders.authorized")
    .RequireAuthorization();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();