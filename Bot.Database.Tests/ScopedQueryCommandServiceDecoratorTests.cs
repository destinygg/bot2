using System;
using Bot.Database.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Database.Tests {
  [TestClass]
  public class ScopedQueryCommandServiceDecoratorTests {
    private Container _container;

    [TestInitialize]
    public void Initialize() {
      var containerManager = new TestContainerManager();
      var databaseInitializer = containerManager.Container.GetInstance<DatabaseInitializer>();
      databaseInitializer.Recreate();
      _container = containerManager.Container;
    }

    [TestMethod]
    public void CreatingBotDbContext_BeforeExecutionContextScopeCreated_ThrowsException() {
      var exception = TestHelper.AssertCatch<ActivationException>(() => _container.GetInstance<IBotDbContext>());

      Assert.AreEqual($"The {nameof(BotDbContext)} is registered as 'Async Scoped' lifestyle, but the instance is requested outside the context of an active (Async Scoped) scope.", exception.Message);
    }

    [TestMethod]
    public void CreatingBotDbContext_WithinExecutionContextScope_DoesNotThrowException() {
      // Should be automatically decorated with ScopedCommandQueryServiceDecorator and therefore in an Execution scope.
      _container.GetInstance<IQueryCommandService<IBotDbContext>>();
    }

    [TestMethod]
    public void CreatingBotDbContext_AfterExecutionContextScopeCreated_ThrowsException() {
      _container.GetInstance<IQueryCommandService<IBotDbContext>>();

      var exception = TestHelper.AssertCatch<ActivationException>(() => _container.GetInstance<IBotDbContext>());

      Assert.AreEqual($"The {nameof(BotDbContext)} is registered as 'Async Scoped' lifestyle, but the instance is requested outside the context of an active (Async Scoped) scope.", exception.Message);
    }

    [TestMethod]
    public void AccessingDbContext_AfterScopeCreatedAndDisposed_ThrowsDisposedException() {
      var contextService = _container.GetInstance<IQueryCommandService<IBotDbContext>>();
      var botDbContext = contextService.Query(db => db);

      var exception = TestHelper.AssertCatch<ObjectDisposedException>(() => botDbContext.AutoPunishments.Find(0));

      Assert.AreEqual($"Cannot access a disposed object. A common cause of this error is disposing a context that was resolved from dependency injection and then later trying to use the same context instance elsewhere in your application. This may occur if you are calling Dispose() on the context, or wrapping the context in a using statement. If you are using dependency injection, you should let the dependency injection container take care of disposing context instances.\r\nObject name: '{nameof(BotDbContext)}'.", exception.Message);
    }

  }
}
