using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Bot.Database.Entities;
using Bot.Models;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class RepositoryPunishmentFactoryTests {

    private Container InitializeContainerAndRepository(string term, [CallerMemberName] string sqliteName = null) {
      var container = new TestContainerManager(configureSettings: settings => {
        settings.MinimumPunishmentSimilarity = 0.7d;
        settings.SqlitePath = sqliteName;
      }).InitializeAndIsolateRepository();
      container.GetInstance<IQueryCommandService<IUnitOfWork>>().Command(db =>
        db.AutoPunishments.Add(new AutoPunishment(new AutoPunishmentEntity {
          Term = term,
          Type = AutoPunishmentType.MutedString,
          Duration = Convert.ToInt64(TimeSpan.FromMinutes(10).TotalSeconds),
        })));
      return container;
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_NotInList_DoesNotPunish() {
      var bannedTerm = TestHelper.RandomString();
      var spokenTerm = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(bannedTerm);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(spokenTerm);
      var RepositoryPunishmentFactory = container.GetInstance<PunishmentFactory>();

      var ban = RepositoryPunishmentFactory.Create(snapshot);

      Assert.IsFalse(ban.Any());
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_FirstOffense_10MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var RepositoryPunishmentFactory = container.GetInstance<PunishmentFactory>();

      var output = RepositoryPunishmentFactory.Create(snapshot);

      var mute = output.Cast<SendableMute>().Single();
      Assert.IsTrue(mute.Duration.TotalMinutes == 10);
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_SecondOffense_20MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var RepositoryPunishmentFactory = container.GetInstance<PunishmentFactory>();

      RepositoryPunishmentFactory.Create(snapshot);
      var output = RepositoryPunishmentFactory.Create(snapshot);

      var mute = output.Cast<SendableMute>().Single();
      Assert.AreEqual(20, mute.Duration.TotalMinutes);
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_ThirdOffense_40MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var RepositoryPunishmentFactory = container.GetInstance<PunishmentFactory>();

      RepositoryPunishmentFactory.Create(snapshot);
      RepositoryPunishmentFactory.Create(snapshot);
      var output = RepositoryPunishmentFactory.Create(snapshot);

      var mute = output.Cast<SendableMute>().Single();
      Assert.AreEqual(40, mute.Duration.TotalMinutes);
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_FourthOffense_80MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var RepositoryPunishmentFactory = container.GetInstance<PunishmentFactory>();

      RepositoryPunishmentFactory.Create(snapshot);
      RepositoryPunishmentFactory.Create(snapshot);
      RepositoryPunishmentFactory.Create(snapshot);
      var output = RepositoryPunishmentFactory.Create(snapshot);

      var mute = output.Cast<SendableMute>().Single();
      Assert.AreEqual(80, mute.Duration.TotalMinutes);
    }

  }
}
