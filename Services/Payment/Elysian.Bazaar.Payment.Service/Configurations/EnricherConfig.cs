using Infrastructure.Logging.Enrichers;
using Serilog;
using Serilog.Configuration;

namespace Configurations;

public static class EnricherConfig
{
    public static LoggerConfiguration WithPaymentEnricher(
        this LoggerEnrichmentConfiguration enrichmentConfiguration,
        string propertyName, 
        string propertyValue,
        IFormatProvider? formatProvider = null)
    {
        return enrichmentConfiguration.With(new PaymentEnricher(propertyName, propertyValue));
    }
}