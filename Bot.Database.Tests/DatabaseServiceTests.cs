using System;
using Bot.Database.Entities;
using Bot.Database.Interfaces;
using Bot.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  [TestClass]
  public class DatabaseServiceTests {
    [TestMethod]
    public void DatabaseServiceAddingIncompleteEntity_Always_ThrowsForeignKeyException() {
      var cm = new ContainerManager();
      var databaseService = cm.Container.GetInstance<IDatabaseService<IBotDbContext>>();
      var databaseInitializer = cm.Container.GetInstance<DatabaseInitializer>();
      databaseInitializer.EnsureDeleted();
      databaseInitializer.EnsureCreated();
      var punishedUser = new PunishedUser {
        Count = 1,
        AutoPunishmentId = 1,
        UserId = 1,
      };

      var exception = TestHelper.AssertCatch<DbUpdateException>(() => databaseService.Command(db => db.PunishedUsers.Add(punishedUser)));

      Assert.AreEqual("SQLite Error 19: 'FOREIGN KEY constraint failed'.", exception.InnerException.Message);
    }

  }
}
