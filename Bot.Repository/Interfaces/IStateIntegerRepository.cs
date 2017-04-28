using System;

namespace Bot.Repository.Interfaces {
  public interface IStateIntegerRepository {
    DateTime LatestStreamOnTime { get; set; }
    DateTime LatestStreamOffTime { get; set; }
    StreamStatus StreamStatus { get; set; }
    long DeathCount { get; set; }
  }

  public enum StreamStatus {
    On,
    Off,
    PossiblyOff
  }
}
