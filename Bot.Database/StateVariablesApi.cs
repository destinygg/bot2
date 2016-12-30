using System;
using Bot.Database.Models;
using Bot.Tools;

namespace Bot.Database {
  public class StateVariablesApi : DbApi {
    public DateTime GetOnTime() => Db.Get<StateVariables>(x => x.Key == MagicStrings.OnTime).Value.FromUnixTime();

    public long SetOnTime(DateTime onTime) {
      var liveObj = new StateVariables {
        Key = MagicStrings.OnTime,
        Value = onTime.ToUnixTime(),
      };
      return Db.Update(liveObj);
    }

  }
}