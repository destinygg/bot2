using System;
using System.Collections.Generic;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class TestableLogger : ILogger {

    public void LogDebug(string message) {
      Outbox.Add(message);
    }

    public void LogInformation(string message) {
      Outbox.Add(message);
    }

    public void LogWarning(string message) {
      Outbox.Add(message);
    }

    public void LogError(string message) {
      Outbox.Add(message);
    }

    public void LogError(string message, Exception exception) {
      Outbox.Add(message);
    }

    public void LogFatal(string message) {
      Outbox.Add(message);
    }

    public void LogFatal(string message, Exception exception) {
      Outbox.Add(message);
    }

    public IList<string> Outbox { get; } = new List<string>();
  }
}
