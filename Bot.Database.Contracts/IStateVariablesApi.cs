using System;

namespace Bot.Database.Contracts {
  public interface IStateIntegerApi {
    DateTime LatestStreamOnTime { get; set; }
  }
}
