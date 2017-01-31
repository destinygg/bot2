using System;

namespace Bot.Api.Interfaces {
  public interface IStateIntegerApi {
    DateTime LatestStreamOnTime { get; set; }
    DateTime LatestStreamOffTime { get; set; }
    long DeathCount { get; set; }
  }
}
