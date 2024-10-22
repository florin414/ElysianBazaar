using Application.Logging;
using Configurations;
using Destructurama;
using Serilog;
using Serilog.Debugging;
using Serilog.Exceptions;
using Serilog.Extensions;
using Serilog.Filters;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Serilog.ILogger;

try
{
    var builder = WebApplication.CreateBuilder(args);

    ApplyHostConfiguration(builder);

    var logger = GetSerilogConfiguration(builder);
    Log.Logger = logger;
    
    ApplyServicesConfigurations(builder.Services);
    builder.Services.AddSingleton(logger);

    var app = builder.Build();

    app.UseApiDocumentation(options =>
    {
        options.UseScalar = true;
    }).Build();
    
    app.MapGet("/test", TestHandler)
        .WithTags("Test");

    await app.RunAsync();
}
finally
{
    SelfLog.Disable();
    await Log.CloseAndFlushAsync();
}

IResult TestHandler(LoggerService loggerService)
{
    loggerService.Run();
    
    return Results.Ok(new { Message = "Hello, World!" });
}
void ApplyHostConfiguration(WebApplicationBuilder builder)
{
    ApplyHostLoggingConfiguration();
    return;

    void ApplyHostLoggingConfiguration()
    {
        builder.Host.ConfiguredLogging(clearProviders: true);
        builder.Host.UseSerilog(); 
    }
}
void ApplyServicesConfigurations(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddApiDocumentation();
    services.AddScoped<LoggerService>();
    //services.AddLogBackgroundService();
}
ILogger GetSerilogConfiguration(WebApplicationBuilder builder)
{
    SelfLog.Enable(msg => Console.Error.WriteLine(msg));
    ILogger logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.Async(x => x.Console(theme:AnsiConsoleTheme.Code), 10)
        //.WriteTo.PaymentSink()
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentUserName()
        .Enrich.WithExceptionDetails()
        .Enrich.WithRequestBody()
        .Enrich.WithPaymentEnricher("CustomName", "CustomProperty")
        .Enrich.WithProperty("RequestId", Guid.NewGuid())
        .Destructure.UsingAttributes()
        .Filter.ByExcluding(logEvent => logEvent.Level == Serilog.Events.LogEventLevel.Debug)
        .Filter.ByExcluding(logEvent => logEvent.Level >= Serilog.Events.LogEventLevel.Error)
        .Filter.ByIncludingOnly(logEvent =>
            logEvent.Properties.ContainsKey("SourceContext") &&
            logEvent.Properties["SourceContext"].ToString().Contains(nameof(LoggerService)))
        //.Filter.ByIncludingOnly(Matching.WithProperty<string>("UserId", userId => userId == "admin"))
        .AuditTo.File(
            "audit-logs.json", 
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
        .CreateLogger();

    return logger;
}