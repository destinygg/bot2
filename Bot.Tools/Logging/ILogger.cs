using System;

namespace Bot.Tools.Logging {
  public interface ILogger {
    void LogDebug(string debug);
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message);
    void LogError(string message, Exception exception);
    void LogFatal(string message);
    void LogFatal(string message, Exception exception);
  }
}
