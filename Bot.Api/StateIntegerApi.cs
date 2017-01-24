using System;
using System.Linq;
using Bot.Tools;

namespace Bot.Api {
  public class StateIntegerApi {

    public DateTime LatestLiveTime {
      get { return _Read(nameof(LatestLiveTime)).FromUnixTime(); }
      set {
        var epochTime = value.ToUnixTime();
        _Write(new StateInteger(nameof(LatestLiveTime), epochTime));
      }
    }

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
