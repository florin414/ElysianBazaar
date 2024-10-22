using Serilog.Core;
using Serilog.Events;

namespace Infrastructure.Logging.Sinks;

public class PaymentSink: ILogEventSink
{
    private readonly IFormatProvider? _formatProvider;

    public PaymentSink(IFormatProvider? formatProvider)
    {
        _formatProvider = formatProvider;
    }
    
    public PaymentSink() : this(null)
    {
    }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        Console.WriteLine($"{DateTime.UtcNow} - {message}");
    }
}
