using System;
namespace Bot.Tools.Logging {

  public abstract class LogEntry {
    public string Message { get; }

    protected LogEntry(string message) {
      Message = message;
    }

  }

  public class DebugLogEntry : LogEntry {
    public DebugLogEntry(string message) : base(message) { }
  }

  public class InformationLogEntry : LogEntry {
    public InformationLogEntry(string message) : base(message) { }
  }

  public class WarningLogEntry : LogEntry {
    public WarningLogEntry(string message) : base(message) { }
  }

  public class ErrorLogEntry : LogEntry {
    public Exception Exception { get; }

    public ErrorLogEntry(string message, Exception exception = null) : base(message) {
      Exception = exception;
    }
  }

  public class FatalLogEntry : LogEntry {
    public Exception Exception { get; }

    public FatalLogEntry(string message, Exception exception = null) : base(message) {
      Exception = exception;
    }
  }

}
