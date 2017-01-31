using Bot.Api.Entities;
using Bot.Api.Interfaces;

namespace Bot.Api {
  public class DatabaseManager {

    public void EnsureCreated() {
      var manager = new BotDbContextManager();
      manager.Save(context => {
        context.Database.EnsureCreated();
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerApi.LatestStreamOnTime), 0));
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerApi.LatestStreamOffTime), 0));
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerApi.DeathCount), 0));
      });
    }

    public void EnsureDeleted() {
      var manager = new BotDbContextManager();
      manager.Save(context => {
        context.Database.EnsureDeleted();
      });
    }

  }
}
