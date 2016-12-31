using System;
using Bot.Database.Models;
using Bot.Pipeline.Contracts;
using Bot.Tools;

namespace Bot.Database {
  public class StateVariablesApi : DbApi {
    private readonly ILogger _logger;

    public StateVariablesApi(ILogger logger) {
      _logger = logger;
    }

    public DateTime OnTime {
      get { return Db.Get<StateVariables>(x => x.Key == MagicStrings.OnTime).Value.FromUnixTime(); }
      set {
        var liveObj = new StateVariables {
          Key = MagicStrings.OnTime,
          Value = value.ToUnixTime(),
        };
        var rowsUpdated = Db.Update(liveObj);
        if (rowsUpdated == 0) {
          _logger.LogWarning("No rows updated for " + MagicStrings.OnTime);
        }

      }
    }

  }
}