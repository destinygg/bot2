using Bot.Database.Entities;
using Bot.Database.Interfaces;
using Bot.Models;
using Bot.Repository.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Tests {
  public class RepositoryInitializer {
    private readonly IQueryCommandService<IBotDbContext> _queryCommandService;
    private readonly DatabaseInitializer _databaseInitializer;

    public RepositoryInitializer(IQueryCommandService<IBotDbContext> queryCommandService) {
      _queryCommandService = queryCommandService;
      _databaseInitializer = new DatabaseInitializer(_queryCommandService);
    }

    private void AddMasterData() {
      _queryCommandService.Command(context => {
        context.StateIntegers.Add(new StateIntegerEntity(nameof(IStateIntegerRepository.LatestStreamOnTime), 0));
        context.StateIntegers.Add(new StateIntegerEntity(nameof(IStateIntegerRepository.LatestStreamOffTime), 0));
        context.StateIntegers.Add(new StateIntegerEntity(nameof(IStateIntegerRepository.StreamStatus), (int) StreamStatus.Off));
        context.StateIntegers.Add(new StateIntegerEntity(nameof(IStateIntegerRepository.DeathCount), 0));
        context.StateIntegers.Add(new StateIntegerEntity(nameof(IStateIntegerRepository.LatestDestinyTweetId), -1));
        context.CustomCommands.Add(new CustomCommandEntity("rules", @"github.com/destinygg/bot2"));
        context.PeriodicMessages.Add(new PeriodicMessageEntity(@"Follow Destiny! twitter.com/OmniDestiny"));
        context.PeriodicMessages.Add(new PeriodicMessageEntity(@"Buy video games with Destiny's GreenManGaming referral link! destiny.gg/gmg"));
      });
    }

    public void RecreateWithMasterData() {
      _databaseInitializer.Recreate();
      AddMasterData();
    }

    public void EnsureDeleted() =>
      _databaseInitializer.EnsureDeleted();
  }
}
