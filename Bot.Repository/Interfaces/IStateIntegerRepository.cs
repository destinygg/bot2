using System;

namespace Bot.Repository.Interfaces {
  public interface IStateIntegerRepository {
    DateTime LatestStreamOnTime { get; set; }
    DateTime LatestStreamOffTime { get; set; }
    long DeathCount { get; set; }
  }
}
