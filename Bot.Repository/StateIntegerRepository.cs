using System;
using System.Linq;
using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class StateIntegerRepository : IStateIntegerRepository {

    private readonly DbSet<StateIntegerEntity> _entities;

    public StateIntegerRepository(DbSet<StateIntegerEntity> entities) {
      _entities = entities;
    }

    public DateTime LatestStreamOnTime {
      get { return _Read(nameof(LatestStreamOnTime)).FromUnixTime(); }
      set { _Update(nameof(LatestStreamOnTime), value.ToUnixTime()); }
    }

    public DateTime LatestStreamOffTime {
      get { return _Read(nameof(LatestStreamOffTime)).FromUnixTime(); }
      set { _Update(nameof(LatestStreamOffTime), value.ToUnixTime()); }
    }

    public StreamStatus StreamStatus {
      get { return (StreamStatus) _Read(nameof(StreamStatus)); }
      set { _Update(nameof(StreamStatus), (int) value); }
    }

    public long DeathCount {
      get { return _Read(nameof(DeathCount)); }
      set { _Update(nameof(DeathCount), value); }
    }

    private long _Read(string key) =>
      _entities.SingleOrDefault(x => x.Key == key).Value;

    private void _Update(string key, long value) =>
      _entities.Update(new StateIntegerEntity(key, value));
  }
}
