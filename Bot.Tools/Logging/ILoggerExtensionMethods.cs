using System;

namespace Bot.Tools.Logging {
  public static class ILoggerExtensionMethods {
    public static void LogDebug(this ILogger logger, string message) =>
      logger.Log(new LogEntry(LoggingEventType.Debug, message));

    public static void LogInformation(this ILogger logger, string message) =>
      logger.Log(new LogEntry(LoggingEventType.Information, message));

    public static void LogWarning(this ILogger logger, string message) =>
      logger.Log(new LogEntry(LoggingEventType.Warning, message));

    public static void LogError(this ILogger logger, string message, Exception exception) =>
      logger.Log(new LogEntry(LoggingEventType.Error, message, exception));

    public static void LogError(this ILogger logger, string message) =>
      logger.Log(new LogEntry(LoggingEventType.Error, message));

    public static void LogFatal(this ILogger logger, string message, Exception exception) =>
      logger.Log(new LogEntry(LoggingEventType.Fatal, message, exception));

    public static void LogFatal(this ILogger logger, string message) =>
      logger.Log(new LogEntry(LoggingEventType.Fatal, message));

  }
}
