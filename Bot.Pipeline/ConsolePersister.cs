using System;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class ConsolePersister : ILogPersister {
    public void Persist(string log) {
      Console.WriteLine(log);
    }
  }
}
