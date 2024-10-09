using System.Text.Json;
using Application.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Configurations;

public static class LoggingConfig
{
    public static IHostBuilder ConfiguredLogging(this IHostBuilder hostBuilder, 
        LogLevel minimumLevel = LogLevel.Information, 
        bool clearProviders = false,
        bool useFilters = false)
    {
        hostBuilder.ConfigureLogging(logging =>
        {
            if (clearProviders)
                logging.ClearProviders();

            logging.SetMinimumLevel(minimumLevel);
            logging.AddConsole();
            logging.AddJsonConsole(x =>
            {
                x.JsonWriterOptions = new JsonWriterOptions
                {
                    Indented = true
                };
            });
            logging.AddProvider(new PaymentLoggerProvider());

            if (useFilters)
                logging
                    .AddFilter("System", LogLevel.Debug)
                    .AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Information)
                    .AddFilter<ConsoleLoggerProvider>("Microsoft.Extensions.Hosting.Internal.Host", LogLevel.Debug);
        });

        return hostBuilder;
    }

    public static IServiceCollection AddLogBackgroundService(this IServiceCollection service)
    {
        service.AddHostedService<LogBackgroundService>();

        return service;
    }
}