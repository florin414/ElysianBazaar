using Serilog.Core;
using Serilog.Events;

namespace Infrastructure.Logging.Enrichers;

public class PaymentEnricher(
    string propertyName, 
    string propertyValue
    ): ILogEventEnricher
{
    private string PropertyName { get; } = propertyName;
    private string PropertyValue { get; } = propertyValue;
    

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var customProperty = propertyFactory.CreateProperty(PropertyName, PropertyValue);
        logEvent.AddPropertyIfAbsent(customProperty);
    }
}