using System;
using Bot.Database.Entities;
using Bot.Database.Interfaces;
using Bot.Tools;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class StateIntegerApi : Repository<StateInteger>, IStateIntegerApi {
    public StateIntegerApi(DbSet<StateInteger> entities) : base(entities) { }

    public DateTime LatestStreamOnTime {
      get { return _Read(nameof(LatestStreamOnTime)).FromUnixTime(); }
      set { _Update(nameof(LatestStreamOnTime), value.ToUnixTime()); }
    }

    public DateTime LatestStreamOffTime {
      get { return _Read(nameof(LatestStreamOffTime)).FromUnixTime(); }
      set { _Update(nameof(LatestStreamOffTime), value.ToUnixTime()); }
    }

    public long DeathCount {
      get { return _Read(nameof(DeathCount)); }
      set { _Update(nameof(DeathCount), value); }
    }

    private long _Read(string key) =>
      SingleOrDefault(x => x.Key == key).Value;

    private void _Update(string key, long value) =>
      Update(new StateInteger(key, value));

  }
}
