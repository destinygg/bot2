using Bot.Database.Entities;

namespace Bot.Api {
  public class InitializeDb {
    public InitializeDb() {
      using (var context = new BotDbContext()) {
        context.Database.EnsureCreated();
        context.StateIntegers.Add(new StateInteger(nameof(StateIntegerApi.LatestStreamOnTime), 0));
        context.StateIntegers.Add(new StateInteger(nameof(StateIntegerApi.LatestStreamOffTime), 0));
        context.StateIntegers.Add(new StateInteger(nameof(StateIntegerApi.DeathCount), 0));
        context.SaveChanges();
      }
    }

  }
}
