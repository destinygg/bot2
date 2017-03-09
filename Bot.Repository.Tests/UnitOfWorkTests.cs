using Bot.Database;
using Bot.Database.Entities;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class UnitOfWorkTests : BaseRepositoryTests {

    [TestMethod]
    public void UpdatePunishedUser() {
      // Arrange
      var nick = TestHelper.RandomString();
      var term = TestHelper.RandomString();
      var type = TestHelper.RandomAutoPunishmentType();
      var duration = TestHelper.RandomInt();
      var oldCount = TestHelper.RandomInt();
      var newCount = oldCount + 1;

      var punishedUser = new PunishedUser {
        User = new User { Nick = nick },
        AutoPunishment = new AutoPunishment {
          Term = term,
          Type = type,
          Duration = duration,
        },
        Count = oldCount,
      };
      using (var unitOfWork = new UnitOfWork(new BotDbContext())) {
        unitOfWork.PunishedUsers.Add(punishedUser);
        unitOfWork.Complete();
      }

      // Act
      using (var unitOfWork = new UnitOfWork(new BotDbContext())) {
        var puEntity = unitOfWork.PunishedUsers.SingleOrDefault(x => x.User.Nick == nick);
        puEntity.Count = newCount;
        unitOfWork.PunishedUsers.Update(puEntity);
        unitOfWork.Complete();
      }

      PunishedUser dbPunishedUser;
      using (var unitOfWork = new UnitOfWork(new BotDbContext())) {
        dbPunishedUser = unitOfWork.PunishedUsers.SingleOrDefault(pu => pu.User.Nick == nick);
      }

      // Assert
      Assert.AreEqual(dbPunishedUser.User.Nick, nick);
      Assert.AreEqual(dbPunishedUser.AutoPunishment.Term, term);
      Assert.AreEqual(dbPunishedUser.AutoPunishment.Type, type);
      Assert.AreEqual(dbPunishedUser.AutoPunishment.Duration, duration);
      Assert.AreEqual(dbPunishedUser.Count, newCount);
    }

  }
}
