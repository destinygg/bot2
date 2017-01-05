using System;

namespace Bot.Models.Contracts {
  public interface IPunishment : ITargetable {
    bool IsPermanent { get; }
    TimeSpan Duration { get; }
  }
}
