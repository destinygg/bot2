using System;

namespace Bot.Tools {
  public class LogEntry {
    public LoggingEventType Severity { get; }
    public string Message { get; }
    public Exception Exception { get; }

    public LogEntry(LoggingEventType severity, string message, Exception exception = null) {
      Severity = severity;
      Message = message;
      Exception = exception;
    }

  }
}
