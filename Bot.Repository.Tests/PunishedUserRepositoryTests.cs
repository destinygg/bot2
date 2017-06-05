using System.Collections.Generic;
using Bot.Database.Entities;
using Bot.Main.Moderate;
using Bot.Models;
using Bot.Repository.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class PunishedUserRepositoryTests {
    [TestMethod]
    public void PunishedUserWithPriors_Increment_IncreasesCountBy1() {
      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var id = TestHelper.RandomInt();
      var term = TestHelper.RandomString();
      var type = TestHelper.RandomAutoPunishmentType();
      var duration = TestHelper.RandomInt();
      var nick = TestHelper.RandomString();
      var count = TestHelper.RandomInt();
      var autoPunishmentEntity = new AutoPunishmentEntity {
        Id = id,
        Term = term,
        Type = type,
        Duration = duration,
      };
      var punishedUsersEntity = new List<PunishedUserEntity> {
        new PunishedUserEntity { Nick = nick, Count = count, AutoPunishmentEntity = autoPunishmentEntity }
      };
      autoPunishmentEntity.PunishedUsers = punishedUsersEntity;
      var repository = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      repository.Command(r => r.AutoPunishments.Add(new AutoPunishment(autoPunishmentEntity)));

      repository.Command(db => db.PunishedUsers.Increment(nick, term));

      var user = repository.Query(db => db.PunishedUsers.GetUser(nick));
      Assert.AreEqual(count + 1, user.Count);
    }

    [TestMethod]
    public void PunishedUserWithNoPriors_Increment_CountIs1() {
      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var id = TestHelper.RandomInt();
      var term = TestHelper.RandomString();
      var type = TestHelper.RandomAutoPunishmentType();
      var duration = TestHelper.RandomInt();
      var nick = TestHelper.RandomString();
      var autoPunishmentEntity = new AutoPunishmentEntity {
        Id = id,
        Term = term,
        Type = type,
        Duration = duration,
      };

      var repository = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      repository.Command(r => r.AutoPunishments.Add(new AutoPunishment(autoPunishmentEntity)));

      repository.Command(db => db.PunishedUsers.Increment(nick, term));

      var user = repository.Query(db => db.PunishedUsers.GetUser(nick));
      Assert.AreEqual(1, user.Count);
    }

  }
}
