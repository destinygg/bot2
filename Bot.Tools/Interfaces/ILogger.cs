using System;

namespace Bot.Tools.Interfaces {
  public interface ILogger {
    void LogWarning(string warning);
    void LogError(string error);
    void LogError(Exception e, string error);
    void LogInformation(string information);
    void LogVerbose(string verbose);
  }
}
