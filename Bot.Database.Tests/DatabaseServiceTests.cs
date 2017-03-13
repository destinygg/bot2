using Bot.Database.Entities;
using Bot.Database.Interfaces;
using Bot.Database.Tests.Helper;
using Bot.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  [TestClass]
  public class DatabaseServiceTests {

    [TestMethod]
    public void DatabaseServiceAddingIncompleteEntity_Always_ThrowsForeignKeyException() {
      var databaseService = DatabaseHelper.GetContainerWithRecreatedAndIsolatedDatabase().GetInstance<IDatabaseService<IBotDbContext>>();

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
