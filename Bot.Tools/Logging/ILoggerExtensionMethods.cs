using System;

namespace Bot.Tools.Logging {
  public static class ILoggerExtensionMethods {
    public static void LogDebug(this ILogger logger, string message) =>
      logger.Log(new DebugLogEntry(message));

    public static void LogInformation(this ILogger logger, string message) =>
      logger.Log(new InformationLogEntry(message));

    public static void LogWarning(this ILogger logger, string message) =>
      logger.Log(new WarningLogEntry(message));

    public static void LogError(this ILogger logger, string message, Exception exception = null) =>
      logger.Log(new ErrorLogEntry(message, exception));

    public static void LogFatal(this ILogger logger, string message, Exception exception = null) =>
      logger.Log(new FatalLogEntry(message, exception));

  }
}
