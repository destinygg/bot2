using System;
using Bot.Database.Entities;

namespace Bot.Database.Interfaces {
  public interface IStateIntegerRepository : IRepository<StateInteger> {
    DateTime LatestStreamOnTime { get; set; }
    DateTime LatestStreamOffTime { get; set; }
    long DeathCount { get; set; }
  }
}
