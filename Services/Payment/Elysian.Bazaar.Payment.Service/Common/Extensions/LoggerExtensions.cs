using System.Diagnostics;
using Common.Models;
using Microsoft.Extensions.Logging;

namespace Common.Extensions;

public static partial class LoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        EventId = 5001,
        Message = "'Generate Func' Customer {Email} purchased product {ProductId} at {Amount}")]
    public static partial void LogPaymentCreation(
        this ILogger logger, string email, decimal amount, int productId);

    public static ILogger Custom(this ILogger logger, string message, params object[] args)
    {
        Debug.Assert(message != null, nameof(message) + " != null");
        logger.Log(CustomLogLevel.Custom, message, args);
        
        return logger;
    }
}

