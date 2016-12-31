using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Pipeline.Contracts;

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
  }
}
