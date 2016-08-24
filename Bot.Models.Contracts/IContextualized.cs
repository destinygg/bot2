using System.Collections.Generic;

namespace Bot.Models.Contracts {
  public interface IContextualized {
    IReceived First { get; }
    IReadOnlyList<IReceived> Context { get; }
  }
}
