using Common.Extensions;
using Common.Logging;
using Common.Models;
using Microsoft.Extensions.Logging;

namespace Application.Logging;

public class LoggerService(ILoggerFactory loggerFactory)
{
    private ILogger<LoggerService> Logger { get; } = loggerFactory.CreateLogger<LoggerService>();
    private static Action<ILogger, string, decimal, int, Exception?> LogPayment { get; } =
        LoggerMessage.Define<string, decimal, int>(
            LogLevel.Information,
            new EventId(5001, nameof(CreatePayment)),
            "Customer {Email} purchased product {ProductId} at {Amount}");

    public void CreatePayment(string email, decimal amount, int productId)
    {
        LogPayment(Logger, email, amount, productId, null);
        Logger.LogPaymentCreation(email, amount, productId);
    }
    public void Run()
    {
        const string name = "Nick";
        const int age = 30;
        Logger.LogInformation(LogEvents.OrderPlaced,"Numele este {Name} și vârsta este {Age}.", name, age);
        
            
        var payment = new Payment
        {
            PaymentId = 1,
            Email = "nick@dometrain.com",
            UserId = Guid.NewGuid(),
            OccuredAt = DateTime.UtcNow
        };

        Logger.LogInformation("Received payment with details {@PaymentData}", payment);
    }
}