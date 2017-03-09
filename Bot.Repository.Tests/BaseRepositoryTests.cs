using Bot.Database;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  public abstract class BaseRepositoryTests {
    private readonly DatabaseInitializer _databaseInitializer;

    protected BaseRepositoryTests() {
      var containerManager = new TestContainerManager();
      _databaseInitializer = containerManager.Container.GetInstance<DatabaseInitializer>();
    }

    [TestInitialize]
    public void Initialize() {
      _databaseInitializer.Recreate();
    }

    [TestCleanup]
    public void Cleanup() {
      _databaseInitializer.EnsureDeleted();
    }

  }
}
