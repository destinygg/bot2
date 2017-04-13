using Bot.Database.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Tests {
  public class DatabaseInitializer {
    private readonly IQueryCommandService<IBotDbContext> _queryCommandService;
    public DatabaseInitializer(IQueryCommandService<IBotDbContext> queryCommandService) {
      _queryCommandService = queryCommandService;
    }

    public void EnsureCreated() {
      _queryCommandService.Command(context => {
        context.Database.EnsureCreated();
      });
    }

    public void EnsureDeleted() {
      _queryCommandService.Command(context => {
        context.Database.EnsureDeleted();
      });
    }

    public void Recreate() {
      EnsureDeleted();
      EnsureCreated();
    }

  }
}
