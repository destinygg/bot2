using System;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class ConsolePersister : ILogPersister {
    public void Persist(string log) {
      Console.WriteLine(log);
    }
  }
}
