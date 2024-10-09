namespace Common.Models;

public static class CustomLogLevel
{
    public static Microsoft.Extensions.Logging.LogLevel Custom  => 
        (Microsoft.Extensions.Logging.LogLevel)41;
}
