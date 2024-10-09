using Application.Logging;
using Configurations;
using Destructurama;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

try
{
    var builder = WebApplication.CreateBuilder(args);

    ApplyHostConfiguration(builder);

    Serilog.ILogger logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.Async(x => x.Console(theme:AnsiConsoleTheme.Code), 10)
        .WriteTo.PaymentSink()
        .Enrich.FromLogContext()
        .Destructure.UsingAttributes()
        .Filter.ByExcluding(logEvent => logEvent.Level == Serilog.Events.LogEventLevel.Debug)
        .Filter.ByIncludingOnly(logEvent =>
            logEvent.Properties.ContainsKey("SourceContext") &&
            logEvent.Properties["SourceContext"].ToString().Contains(nameof(LoggerService))) 
        .CreateLogger();

    Log.Logger = logger;

    ApplyServicesConfigurations(builder.Services);
    builder.Services.AddSingleton(logger);

    var app = builder.Build();

    app.UseConfiguredSwagger();
    app.MapGet("/test", TestHandler)
        .WithTags("Test");

    await app.RunAsync();
}
finally
{
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
    services.AddConfiguredSwagger();
    services.AddScoped<LoggerService>();
    //services.AddLogBackgroundService();
}