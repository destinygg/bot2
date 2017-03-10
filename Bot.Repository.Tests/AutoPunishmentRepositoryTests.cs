using System.Collections.Generic;
using System.Linq;
using Bot.Database;
using Bot.Database.Entities;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class AutoPunishmentRepositoryTests {

    [TestMethod]
    public void ReadWriteAutoPunishment() {
      var container = RepositoryHelper.GetContainer();

      var term = TestHelper.RandomString();
      var type = TestHelper.RandomAutoPunishmentType();
      var duration = TestHelper.RandomInt();
      using (var context = container.GetInstance<BotDbContext>()) {
        var autoPunishmentRepository = new AutoPunishmentRepository(context.AutoPunishments);
        autoPunishmentRepository.Add(new AutoPunishment {
          Term = term,
          Type = type,
          Duration = duration,
        });
        context.SaveChanges();
      }

      IEnumerable<AutoPunishment> testRead;
      using (var context = container.GetInstance<BotDbContext>()) {
        var userRepository = new AutoPunishmentRepository(context.AutoPunishments);
        testRead = userRepository.GetAll();
      }
      var dbAutoPunishment = testRead.First();

      Assert.AreEqual(dbAutoPunishment.Term, term);
      Assert.AreEqual(dbAutoPunishment.Type, type);
      Assert.AreEqual(dbAutoPunishment.Duration, duration);
    }

  }
}
