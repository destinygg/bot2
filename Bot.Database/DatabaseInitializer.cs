using Bot.Database.Entities;
using Bot.Database.Interfaces;

namespace Bot.Database {
  public class DatabaseInitializer {
    private readonly IDatabaseService<IBotDbContext> _databaseService;
    public DatabaseInitializer(IDatabaseService<IBotDbContext> databaseService) {
      _databaseService = databaseService;
    }

    public void EnsureCreated() {
      _databaseService.Command(context => {
        context.Database.EnsureCreated();
      });
    }

    public void AddMasterData() {
      _databaseService.Command(context => {
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerRepository.LatestStreamOnTime), 0));
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerRepository.LatestStreamOffTime), 0));
        context.StateIntegers.Add(new StateInteger(nameof(IStateIntegerRepository.DeathCount), 0));
      });
    }

    public void EnsureDeleted() {
      _databaseService.Command(context => {
        context.Database.EnsureDeleted();
      });
    }

  }
}
