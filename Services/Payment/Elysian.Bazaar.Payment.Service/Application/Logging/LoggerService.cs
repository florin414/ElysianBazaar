using Common.Extensions;
using Common.Models;
using Infrastructure.Logging;
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
        
        using (Logger.BeginScope(new Dictionary<string, bool> { ["IsAudit"] = true }))
        {
            Logger.LogError("This is an audit log for an error!");
        }

        var transactionId = 4873249837248732;
        
        using (Logger.BeginScope(new Dictionary<string, object> { ["TransactionId"] = transactionId }))
        {
            Logger.LogInformation("Transaction started.");
            Logger.LogInformation("Transaction completed.");
        }
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