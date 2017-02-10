using Bot.Database.Entities;
using Bot.Database.Interfaces;

namespace Bot.Database.Tests {
  public class DatabaseInitializer {

    public void EnsureCreated() {
      var manager = new BotDbContextManager();
      manager.Save(context => {
        context.Database.EnsureCreated();
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerRepository.LatestStreamOnTime), 0));
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerRepository.LatestStreamOffTime), 0));
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerRepository.DeathCount), 0));
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
