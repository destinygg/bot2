using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  public abstract class BaseRepositoryTests {
    private readonly DatabaseInitializer _databaseInitializer;

    protected BaseRepositoryTests() {
      var containerManager = new ContainerManager();
      _databaseInitializer = containerManager.Container.GetInstance<DatabaseInitializer>();
    }

    [TestInitialize]
    public void Initialize() {
      _databaseInitializer.RecreateWithMasterData();
    }

    [TestCleanup]
    public void Cleanup() {
      _databaseInitializer.EnsureDeleted();
    }

  }
}
