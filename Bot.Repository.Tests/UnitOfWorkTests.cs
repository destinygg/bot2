using Bot.Database;
using Bot.Database.Entities;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class UnitOfWorkTests {

    [TestMethod]
    public void UpdatePunishedUser() {
      var container = RepositoryHelper.GetContainer();

      var nick = TestHelper.RandomString();
      var term = TestHelper.RandomString();
      var type = TestHelper.RandomAutoPunishmentType();
      var duration = TestHelper.RandomInt();
      var oldCount = TestHelper.RandomInt();
      var punishedUserWrite = new PunishedUser {
        User = new User { Nick = nick },
        AutoPunishment = new AutoPunishment {
          Term = term,
          Type = type,
          Duration = duration,
        },
        Count = oldCount,
      };
      using (var unitOfWork = new UnitOfWork(container.GetInstance<BotDbContext>())) {
        unitOfWork.PunishedUsers.Add(punishedUserWrite);
        unitOfWork.SaveChanges();
      }

      var newCount = oldCount + 1;
      using (var unitOfWork = new UnitOfWork(container.GetInstance<BotDbContext>())) {
        var punishedUserUpdate = unitOfWork.PunishedUsers.SingleOrDefault(pu => pu.User.Nick == nick);
        punishedUserUpdate.Count = newCount;
        unitOfWork.PunishedUsers.Update(punishedUserUpdate);
        unitOfWork.SaveChanges();
      }

      PunishedUser punishedUserRead;
      using (var unitOfWork = new UnitOfWork(container.GetInstance<BotDbContext>())) {
        punishedUserRead = unitOfWork.PunishedUsers.SingleOrDefault(pu => pu.User.Nick == nick);
      }

      Assert.AreEqual(punishedUserRead.User.Nick, nick);
      Assert.AreEqual(punishedUserRead.AutoPunishment.Term, term);
      Assert.AreEqual(punishedUserRead.AutoPunishment.Type, type);
      Assert.AreEqual(punishedUserRead.AutoPunishment.Duration, duration);
      Assert.AreEqual(punishedUserRead.Count, newCount);
    }

  }
}
