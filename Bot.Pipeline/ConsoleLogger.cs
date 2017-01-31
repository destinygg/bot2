using System;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class ConsoleLogger : ILogger {
    public void LogWarning(string warning) {
      Console.WriteLine(warning);
    }

    public void LogError(string error) {
      Console.WriteLine(error);
    }

    public void LogInformation(string information) {
      Console.WriteLine(information);
    }

    public void LogVerbose(string verbose) {
      Console.WriteLine(verbose);
    }
  }
}
