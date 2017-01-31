using System;

namespace Bot.Tools.Interfaces {
  public interface ITimeService {
    DateTime UtcNow { get; }
    DateTime DestinyNow { get; }
    DateTime DebuggerNow { get; }
  }
}
