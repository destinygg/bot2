using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  public abstract class BaseRepositoryTests {
    private readonly RepositoryInitializer _repositoryInitializer;

    protected BaseRepositoryTests() {
      var containerManager = new TestContainerManager();
      _repositoryInitializer = containerManager.Container.GetInstance<RepositoryInitializer>();
    }

    [TestInitialize]
    public void Initialize() {
      _repositoryInitializer.RecreateWithMasterData();
    }

    [TestCleanup]
    public void Cleanup() {
      _repositoryInitializer.EnsureDeleted();
    }

  }
}
