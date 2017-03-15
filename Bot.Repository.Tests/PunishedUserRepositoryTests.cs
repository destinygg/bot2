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
      var container = RepositoryHelper.GetContainerWithInitializedAndIsolatedRepository();

      var nick = TestHelper.RandomString();
      var term = TestHelper.RandomString();
      var type = TestHelper.RandomAutoPunishmentType();
      var duration = TestHelper.RandomInt();
      var count = TestHelper.RandomInt();
      var punishedUserWrite = new PunishedUserEntity {
        UserEntity = new UserEntity { Nick = nick },
        AutoPunishmentEntity = new AutoPunishmentEntity {
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

      IEnumerable<PunishedUserEntity> testRead;
      using (var context = container.GetInstance<BotDbContext>()) {
        var userRepository = new PunishedUserRepository(context.PunishedUsers);
        testRead = userRepository.GetAllWithIncludes();
      }

      var testReadSingle = testRead.Single();
      Assert.AreEqual(testReadSingle.UserEntity.Nick, nick);
      Assert.AreEqual(testReadSingle.AutoPunishmentEntity.Term, term);
      Assert.AreEqual(testReadSingle.AutoPunishmentEntity.Type, type);
      Assert.AreEqual(testReadSingle.AutoPunishmentEntity.Duration, duration);
      Assert.AreEqual(testReadSingle.Count, count);
    }

  }
}
