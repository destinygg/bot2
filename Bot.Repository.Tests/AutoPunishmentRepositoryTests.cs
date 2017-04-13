using System.Collections.Generic;
using System.Linq;
using Bot.Database.Entities;
using Bot.Models;
using Bot.Repository.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class AutoPunishmentRepositoryTests {

    [TestMethod]
    public void ReadWriteAutoPunishment() {
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

      var testRead = new List<AutoPunishment>();
      repository.Command(r => {
        testRead.AddRange(r.AutoPunishments.GetAllMutedString());
        testRead.AddRange(r.AutoPunishments.GetAllBannedString());
        testRead.AddRange(r.AutoPunishments.GetAllMutedRegex());
        testRead.AddRange(r.AutoPunishments.GetAllBannedRegex());
      });
      var dbAutoPunishment = testRead.Single();

      Assert.AreEqual(dbAutoPunishment.Id, id);
      Assert.AreEqual(dbAutoPunishment.Term, term);
      Assert.AreEqual(dbAutoPunishment.Type, type);
      Assert.AreEqual(dbAutoPunishment.Duration.TotalSeconds, duration);
      Assert.AreEqual(dbAutoPunishment.PunishedUsers.Single().Count, count);
      Assert.AreEqual(dbAutoPunishment.PunishedUsers.Single().Nick, nick);
    }

    [TestMethod]
    public void ReadWriteUpdateAutoPunishment() {
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
      var repository = container.GetInstance<IQueryCommandService<IUnitOfWork>>();

      repository.Command(r => r.AutoPunishments.Add(new AutoPunishment(autoPunishmentEntity)));

      autoPunishmentEntity.Id = TestHelper.RandomInt();
      autoPunishmentEntity.Term = TestHelper.RandomString();
      autoPunishmentEntity.Type = TestHelper.RandomAutoPunishmentType();
      autoPunishmentEntity.Duration = TestHelper.RandomInt();
      nick = TestHelper.RandomString();
      count = TestHelper.RandomInt();
      autoPunishmentEntity = new AutoPunishmentEntity {
        Id = id,
        Term = term,
        Type = type,
        Duration = duration,
      };
      punishedUsersEntity = new List<PunishedUserEntity> {
        new PunishedUserEntity { Nick = nick, Count = count, AutoPunishmentEntity = autoPunishmentEntity }
      };
      autoPunishmentEntity.PunishedUsers = punishedUsersEntity;

      repository.Command(r => r.AutoPunishments.Update(new AutoPunishment(autoPunishmentEntity)));

      var testRead = new List<AutoPunishment>();
      repository.Command(r => {
        testRead.AddRange(r.AutoPunishments.GetAllMutedString());
        testRead.AddRange(r.AutoPunishments.GetAllBannedString());
        testRead.AddRange(r.AutoPunishments.GetAllMutedRegex());
        testRead.AddRange(r.AutoPunishments.GetAllBannedRegex());
      });
      var dbAutoPunishment = testRead.Single();

      Assert.AreEqual(dbAutoPunishment.Id, id);
      Assert.AreEqual(dbAutoPunishment.Term, term);
      Assert.AreEqual(dbAutoPunishment.Type, type);
      Assert.AreEqual(dbAutoPunishment.Duration.TotalSeconds, duration);
      Assert.AreEqual(dbAutoPunishment.PunishedUsers.Single().Count, count);
      Assert.AreEqual(dbAutoPunishment.PunishedUsers.Single().Nick, nick);
    }

  }
}
