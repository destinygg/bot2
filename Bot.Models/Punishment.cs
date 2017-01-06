using System;

namespace Bot.Models.Contracts {
  public interface IPunishment : ITargetable {
    TimeSpan Duration { get; }
    string Reason { get; }

  }
}
