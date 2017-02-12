using Bot.Database.Interfaces;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Database.Tests {
  [TestClass]
  public class ScopedDatabaseServiceDecoratorTests {
    private Container _container;

    [TestInitialize]
    public void Initialize() {
      var containerManager = new ContainerManager();
      var databaseInitializer = containerManager.Container.GetInstance<DatabaseInitializer>();
      databaseInitializer.RecreateWithMasterData();
      _container = containerManager.Container;
    }

    [TestMethod]
    public void CreatingBotDbContext_BeforeExecutionContextScopeExists_ThrowsException() {
      var exception = TestHelper.AssertCatch<ActivationException>(() => _container.GetInstance<IBotDbContext>());

      Assert.AreEqual($"The {nameof(IBotDbContext)} is registered as 'Execution Context Scope' lifestyle, but the instance is requested outside the context of a Execution Context Scope.", exception.Message);
    }

    [TestMethod]
    public void CreatingBotDbContext_WithinExecutionContextScopeExists_DoesNotThrowException() {
      // Should be automatically decorated with ScopedDatabaseServiceDecorator and therefore in an Execution scope.
      _container.GetInstance<IDatabaseService<IBotDbContext>>();
    }

    [TestMethod]
    public void CreatingBotDbContext_AfterExecutionContextScopeCreated_ThrowsException() {
      _container.GetInstance<IDatabaseService<IBotDbContext>>();

      var exception = TestHelper.AssertCatch<ActivationException>(() => _container.GetInstance<IBotDbContext>());

      Assert.AreEqual($"The {nameof(IBotDbContext)} is registered as 'Execution Context Scope' lifestyle, but the instance is requested outside the context of a Execution Context Scope.", exception.Message);
    }

  }
}
