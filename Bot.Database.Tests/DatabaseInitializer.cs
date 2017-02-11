using Bot.Database.Entities;
using Bot.Database.Interfaces;

namespace Bot.Database.Tests {
  public class DatabaseInitializer {
    private readonly BotDbContextManager _contextManager;

    public DatabaseInitializer(BotDbContextManager contextManager) {
      _contextManager = contextManager;
    }

    public void EnsureCreated() {
      _contextManager.Save(context => {
        context.Database.EnsureCreated();
      });
    }

    public void AddMasterData() {
      _contextManager.Save(context => {
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerRepository.LatestStreamOnTime), 0));
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerRepository.LatestStreamOffTime), 0));
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerRepository.DeathCount), 0));
      });
    }

    public void EnsureDeleted() {
      _contextManager.Save(context => {
        context.Database.EnsureDeleted();
      });
    }

  }
}
