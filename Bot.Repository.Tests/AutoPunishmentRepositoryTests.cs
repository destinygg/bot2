using System.Collections.Generic;
using System.Linq;
using Bot.Database;
using Bot.Database.Entities;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class AutoPunishmentRepositoryTests : BaseRepositoryTests {

    [TestMethod]
    public void ReadWriteAutoPunishment() {
      // Arrange
      var term = TestHelper.RandomString();
      var type = TestHelper.RandomAutoPunishmentType();
      var duration = TestHelper.RandomInt();

      // Act
      using (var context = new BotDbContext()) {
        var autoPunishmentRepository = new AutoPunishmentRepository(context.AutoPunishments);
        autoPunishmentRepository.Add(new AutoPunishment {
          Term = term,
          Type = type,
          Duration = duration,
        });
        context.SaveChanges();
      }

      IEnumerable<AutoPunishment> testRead;
      using (var context = new BotDbContext()) {
        var userRepository = new AutoPunishmentRepository(context.AutoPunishments);
        testRead = userRepository.GetAll();
      }
      var dbAutoPunishment = testRead.First();

      // Assert
      Assert.AreEqual(dbAutoPunishment.Duration, duration);
      Assert.AreEqual(dbAutoPunishment.Term, term);
      Assert.AreEqual(dbAutoPunishment.Type, type);
    }

  }
}
