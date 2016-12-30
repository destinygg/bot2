using System;
using Bot.Database.Models;
using Bot.Tools;

namespace Bot.Database {
  public class StateVariablesApi : DbApi {
    public DateTime OnTime {
      get { return Db.Get<StateVariables>(x => x.Key == MagicStrings.OnTime).Value.FromUnixTime(); }
      set {
        var liveObj = new StateVariables {
          Key = MagicStrings.OnTime,
          Value = value.ToUnixTime(),
        };
        Db.Update(liveObj);
      }
    }

  }
}