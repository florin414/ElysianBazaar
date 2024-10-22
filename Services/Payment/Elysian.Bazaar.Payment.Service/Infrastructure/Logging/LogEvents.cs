using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging;

public static class LogEvents
{
   public static readonly EventId UserCreated = new (1000, "UserCreated");
   public static readonly EventId UserDeleted = new (1001, "UserDeleted");
   public static readonly EventId OrderPlaced = new (2000, "OrderPlaced");
   public static readonly EventId OrderCancelled = new (2001, "OrderCancelled");
   public static readonly EventId SystemError = new (5000, "SystemError");
}