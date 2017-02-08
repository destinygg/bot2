using System.Collections.Generic;
using System.Linq;
using Bot.Database.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  [TestClass]
  public class PunishedUserRepositoryTests : BaseRepositoryTests {

    [TestMethod]
    public void PunishedUserWriteAndGetAllWithIncludes() {
      // Arrange
      var nick = RandomString();
      var term = RandomString();
      var type = RandomAutoPunishmentType();
      var duration = RandomInt();
      var count = RandomInt();

      // Act
      using (var context = new BotDbContext()) {
        var autoPunishmentRepository = new PunishedUserRepository(context.PunishedUsers);
        autoPunishmentRepository.Add(new PunishedUser {
          User = new User { Nick = nick },
          AutoPunishment = new AutoPunishment {
            Term = term,
            Type = type,
            Duration = duration,
          },
          Count = count,
        });
        context.SaveChanges();
      }

      IEnumerable<PunishedUser> testRead;
      using (var context = new BotDbContext()) {
        var userRepository = new PunishedUserRepository(context.PunishedUsers);
        testRead = userRepository.GetAllWithIncludes();
      }
      var dbPunishedUser = testRead.Single();

      // Assert
      Assert.AreEqual(dbPunishedUser.User.Nick, nick);
      Assert.AreEqual(dbPunishedUser.AutoPunishment.Term, term);
      Assert.AreEqual(dbPunishedUser.AutoPunishment.Type, type);
      Assert.AreEqual(dbPunishedUser.AutoPunishment.Duration, duration);
      Assert.AreEqual(dbPunishedUser.Count, count);
    }

  }
}
