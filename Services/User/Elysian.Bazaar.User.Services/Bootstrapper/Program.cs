using Application;
using Contracts.Events;
using Infrastructure.Npg.Contexts;
using Infrastructure.RabbitMQ;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AccountsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AccountsDb")));

builder.Services.AddSingleton(new RabbitMqPublisher(builder.Configuration.GetConnectionString("RabbitMq")!));
builder.Services.AddScoped<CreateClientRequestedHandler>();
builder.Services.AddScoped<RabbitMqConsumer<CreateClientRequested>>(
    sp => new RabbitMqConsumer<CreateClientRequested>
    (builder.Configuration.GetConnectionString("RabbitMq")!,
        sp.GetRequiredService<CreateClientRequestedHandler>()));

var app = builder.Build();

EnsureDbCreated(app);

app.MapGet("/test", TestHandler);
app.MapGet("/", () => "Accounts");

using var scope = app.Services.CreateScope();
var rabbitMqConsumer = scope.ServiceProvider.GetRequiredService<RabbitMqConsumer<CreateClientRequested>>();
rabbitMqConsumer.StartConsuming("clients.events", "accounts.create_account");

app.Run();

Results<Ok<string>, BadRequest<string>> TestHandler()
{
    string? message = "Hello, World!";

    if (message == null)
    {
        return TypedResults.BadRequest("message is null");
    }
    
    return TypedResults.Ok("Hello, World!");
}

static void EnsureDbCreated(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var scopedServices = scope.ServiceProvider;
    var context = scopedServices.GetRequiredService<AccountsDbContext>();
    context.Database.EnsureCreated();
}