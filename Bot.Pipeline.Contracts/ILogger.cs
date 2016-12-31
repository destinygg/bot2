using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Pipeline.Contracts {
  public interface ILogger {
    void LogWarning(string warning);
    void LogError(string error);
    void LogInformation(string information);
  }
}
