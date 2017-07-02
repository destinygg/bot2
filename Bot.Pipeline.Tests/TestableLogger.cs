using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Tools.Logging;

namespace Bot.Pipeline.Tests {
  public class TestableLogger : ILogger {

    public void LogDebug(string message) {
      LeveledOutbox.Add(Tuple.Create(message, Level.Debug));
    }

    public void LogInformation(string message) {
      LeveledOutbox.Add(Tuple.Create(message, Level.Information));
    }

    public void LogWarning(string message) {
      LeveledOutbox.Add(Tuple.Create(message, Level.Warning));
    }

    public void LogError(string message) {
      LeveledOutbox.Add(Tuple.Create(message, Level.Error));
    }

    public void LogError(string message, Exception exception) {
      LeveledOutbox.Add(Tuple.Create(message, Level.Error));
    }

    public void LogFatal(string message) {
      LeveledOutbox.Add(Tuple.Create(message, Level.Fatal));
    }

    public void LogFatal(string message, Exception exception) {
      LeveledOutbox.Add(Tuple.Create(message, Level.Fatal));
    }

    public IList<string> DebugOutbox => LeveledOutbox.Where(x => x.Item2 >= Level.Debug).Select(x => x.Item1).ToList();

    public IList<string> InformationOutbox => LeveledOutbox.Where(x => x.Item2 >= Level.Information).Select(x => x.Item1).ToList();

    public IList<string> WarningOutbox => LeveledOutbox.Where(x => x.Item2 >= Level.Warning).Select(x => x.Item1).ToList();

    public IList<string> ErrorOutbox => LeveledOutbox.Where(x => x.Item2 >= Level.Error).Select(x => x.Item1).ToList();

    public IList<string> FatalOutbox => LeveledOutbox.Where(x => x.Item2 >= Level.Fatal).Select(x => x.Item1).ToList();

    public IList<string> Outbox => LeveledOutbox.Select(x => x.Item1).ToList();

    public IList<Tuple<string, Level>> LeveledOutbox { get; } = new List<Tuple<string, Level>>();

    public enum Level { Debug, Information, Warning, Error, Fatal }
  }
}
