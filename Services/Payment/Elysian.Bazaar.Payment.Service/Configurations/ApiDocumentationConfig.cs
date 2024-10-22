using AspNetCore.Scalar;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Configurations;

public class ApiDocumentationOptions
{
    public bool UseSwagger { get; set; } = false;
    public bool UseScalar { get; set; } = false;
}

public static class ApiDocumentationConfig
{
    private static ApiDocumentationOptions Options { get; set; } = new ();
    public static IApplicationBuilder UseApiDocumentation(this IApplicationBuilder app, Action<ApiDocumentationOptions> configureOptions)
    {
        configureOptions(Options);

        if (Options.UseSwagger)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        if (Options.UseScalar)
        {
            app.UseSwagger();
            app.UseScalar(options =>
            {
                options.DocumentTitle = "Payment Service Api";
                options.UseTheme(Theme.Mars);
                options.UseLayout(Layout.Modern);
            });
        }

        return app;
    }
    
    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
            { 
                Title = "Payment.Service.Api",
                Version = "v1",
            });
        });

        return services;
    }
}