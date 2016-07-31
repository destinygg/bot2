using System;

namespace Bot.Models.Contracts {
  public interface IPunishment : IHaveTarget {
    bool IsPermanent { get; }
    TimeSpan Duration { get; }
  }
}
