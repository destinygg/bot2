using Bot.Database.Entities;
using Bot.Database.Interfaces;
using Bot.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  [TestClass]
  public class DatabaseServiceTests {
    private IDatabaseService<IBotDbContext> _databaseService;

    [TestInitialize]
    public void Initialize() {
      var containerManager = new TestContainerManager();
      var databaseInitializer = containerManager.Container.GetInstance<DatabaseInitializer>();
      databaseInitializer.RecreateWithMasterData();
      _databaseService = containerManager.Container.GetInstance<IDatabaseService<IBotDbContext>>();
    }

    [TestMethod]
    public void DatabaseServiceAddingIncompleteEntity_Always_ThrowsForeignKeyException() {
      var punishedUser = new PunishedUser {
        Count = 1,
        AutoPunishmentId = 1,
        UserId = 1,
      };

      var exception = TestHelper.AssertCatch<DbUpdateException>(() => _databaseService.Command(db => db.PunishedUsers.Add(punishedUser)));

      Assert.AreEqual("SQLite Error 19: 'FOREIGN KEY constraint failed'.", exception.InnerException.Message);
    }

  }
}
