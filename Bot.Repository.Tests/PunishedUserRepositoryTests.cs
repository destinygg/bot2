using System.Collections.Generic;
using System.Linq;
using Bot.Database;
using Bot.Database.Entities;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class PunishedUserRepositoryTests {

    [TestMethod]
    public void PunishedUserWriteAndGetAllWithIncludes() {
      var container = RepositoryHelper.GetContainer(nameof(PunishedUserWriteAndGetAllWithIncludes));

      var nick = TestHelper.RandomString();
      var term = TestHelper.RandomString();
      var type = TestHelper.RandomAutoPunishmentType();
      var duration = TestHelper.RandomInt();
      var count = TestHelper.RandomInt();
      var punishedUserWrite = new PunishedUser {
        User = new User { Nick = nick },
        AutoPunishment = new AutoPunishment {
          Term = term,
          Type = type,
          Duration = duration,
        },
        Count = count,
      };
      using (var context = container.GetInstance<BotDbContext>()) {
        var autoPunishmentRepository = new PunishedUserRepository(context.PunishedUsers);
        autoPunishmentRepository.Add(punishedUserWrite);
        context.SaveChanges();
      }

      IEnumerable<PunishedUser> testRead;
      using (var context = container.GetInstance<BotDbContext>()) {
        var userRepository = new PunishedUserRepository(context.PunishedUsers);
        testRead = userRepository.GetAllWithIncludes();
      }

      var testReadSingle = testRead.Single();
      Assert.AreEqual(testReadSingle.User.Nick, nick);
      Assert.AreEqual(testReadSingle.AutoPunishment.Term, term);
      Assert.AreEqual(testReadSingle.AutoPunishment.Type, type);
      Assert.AreEqual(testReadSingle.AutoPunishment.Duration, duration);
      Assert.AreEqual(testReadSingle.Count, count);
    }

  }
}
