using System;

namespace Bot.Tools.Contracts {
  public interface ITimeService {
    DateTime UtcNow { get; }
    DateTime DestinyNow { get; }
    DateTime DebuggerNow { get; }
  }
}
