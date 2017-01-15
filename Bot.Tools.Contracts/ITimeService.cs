using System;

namespace Bot.Tools.Contracts {
  public interface ITimeService {
    DateTime UtcNow { get; }
  }
}
