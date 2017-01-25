using System;
using System.Linq;
using Bot.Database.Entities;
using Bot.Tools;

namespace Bot.Api {
  public class StateIntegerApi{

    public DateTime LatestStreamOnTime {
      get { return _Read(nameof(LatestStreamOnTime)).FromUnixTime(); }
      set {
        var epochTime = value.ToUnixTime();
        _Write(new StateInteger(nameof(LatestStreamOnTime), epochTime));
      }
    }

    public DateTime LatestStreamOffTime { get; set; }
    public int DeathCount { get; set; }

    private long _Read(string key) {
      using (var context = new BotDbContext()) {
        return context.StateIntegers.First(si => si.Key == key).Value;
      }
    }

    private int _Write(StateInteger stateInteger) {
      using (var context = new BotDbContext()) {
        context.StateIntegers.Update(stateInteger);
        return context.SaveChanges();
      }
    }

  }
}
