using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  public abstract class BaseRepositoryTests {

    [TestInitialize]
    public void Initialize() {
      var manager = new DatabaseManager();
      manager.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup() {
      var manager = new DatabaseManager();
      manager.EnsureDeleted();
    }

  }
}
