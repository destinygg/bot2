using Bot.Database.Interfaces;

namespace Bot.Database.Tests.Helper {
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

    public void EnsureDeleted() {
      _databaseService.Command(context => {
        context.Database.EnsureDeleted();
      });
    }

    public void Recreate() {
      EnsureDeleted();
      EnsureCreated();
    }

  }
}
