using Infrastructure.Logging;
using Infrastructure.Logging.Loggers;
using Microsoft.Extensions.Logging;

namespace Application.Logging;

public class PaymentLoggerProvider: ILoggerProvider
{
    public void Dispose() { }

    public ILogger CreateLogger(string categoryName)
    {
        return new PaymentLogger();
    }
}