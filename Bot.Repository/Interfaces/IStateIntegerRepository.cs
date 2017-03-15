using System;
using Bot.Database.Entities;

namespace Bot.Repository.Interfaces {
  public interface IStateIntegerRepository : IRepository<StateIntegerEntity> {
    DateTime LatestStreamOnTime { get; set; }
    DateTime LatestStreamOffTime { get; set; }
    long DeathCount { get; set; }
  }
}
