using System;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class Logger : ILogger {
    private readonly ILogFormatter _logFormatter;
    private readonly ILogPersister _logPersister;

    public Logger(ILogFormatter logFormatter, ILogPersister logPersister) {
      _logFormatter = logFormatter;
      _logPersister = logPersister;
    }

    public void LogWarning(string warning) => _logPersister.Persist(_logFormatter.FormatWarning(warning));
    public void LogError(string error) => _logPersister.Persist(_logFormatter.FormatError(error));
    public void LogError(Exception e, string error) => _logPersister.Persist(_logFormatter.FormatError(e, error));
    public void LogInformation(string information) => _logPersister.Persist(_logFormatter.FormatInformation(information));
    public void LogVerbose(string verbose) => _logPersister.Persist(_logFormatter.FormatVerbose(verbose));
  }
}
