using Bot.Database.Contracts;
using Bot.Database.Entities;

namespace Bot.Api {
  public class InitializeDb {
    public InitializeDb() {
      var manager = new BotDbContextManager();
      manager.CallWithForeignKeysAndSaving(context => {
        context.Database.EnsureCreated();
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerApi.LatestStreamOnTime), 0));
        //context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerApi.LatestStreamOffTime), 0));
        //context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerApi.DeathCount), 0));
      });
    }

  }
}
