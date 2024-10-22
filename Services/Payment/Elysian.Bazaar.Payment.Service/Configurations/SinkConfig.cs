using Infrastructure.Logging.Sinks;
using Serilog;
using Serilog.Configuration;

namespace Configurations;

public static class SinkConfig
{
    public static LoggerConfiguration PaymentSink(
        this LoggerSinkConfiguration sinkConfiguration,
        IFormatProvider? formatProvider = null)
    {
        return sinkConfiguration.Sink(new PaymentSink());
    }
}