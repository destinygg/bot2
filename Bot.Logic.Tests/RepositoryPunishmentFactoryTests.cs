using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Bot.Database.Entities;
using Bot.Main.Moderate;
using Bot.Models;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tests;
using Bot.Tools;
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
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      var ban = punishmentFactory.Create(snapshot);

      Assert.IsFalse(ban.Any());
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_FirstOffense_10MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      var output = punishmentFactory.Create(snapshot);

      var mute = output.OfType<SendableMute>().Single();
      Assert.AreEqual(TimeSpan.FromMinutes(10), mute.Duration);
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_SecondOffense_20MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      punishmentFactory.Create(snapshot);
      var output = punishmentFactory.Create(snapshot);

      var mute = output.OfType<SendableMute>().Single();
      Assert.AreEqual(TimeSpan.FromMinutes(20), mute.Duration);
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_ThirdOffense_40MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      punishmentFactory.Create(snapshot);
      punishmentFactory.Create(snapshot);
      var output = punishmentFactory.Create(snapshot);

      var mute = output.OfType<SendableMute>().Single();
      Assert.AreEqual(TimeSpan.FromMinutes(40), mute.Duration);
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_FourthOffense_80MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      punishmentFactory.Create(snapshot);
      punishmentFactory.Create(snapshot);
      punishmentFactory.Create(snapshot);
      var output = punishmentFactory.Create(snapshot);

      var mute = output.OfType<SendableMute>().Single();
      Assert.AreEqual(TimeSpan.FromMinutes(80), mute.Duration);
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_FirstOffense_OrdinaryBanReason() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      var output = punishmentFactory.Create(snapshot);

      var reason = output.Skip(1).Cast<SendablePublicMessage>().Single().Text;
      Assert.AreEqual("10m for prohibited phrase", reason);

    }

    [TestMethod]
    public void RepositoryPunishmentFactory_SecondOffense_SecondBanReason() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      punishmentFactory.Create(snapshot);
      var output = punishmentFactory.Create(snapshot);

      var reason = output.Skip(1).Cast<SendablePublicMessage>().Single().Text;
      Assert.AreEqual("20m for prohibited phrase; your time has doubled. Future sanctions will not be explicitly justified.", reason);
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_ThirdOffense_NoBanReason() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      punishmentFactory.Create(snapshot);
      punishmentFactory.Create(snapshot);
      var output = punishmentFactory.Create(snapshot);

      Console.WriteLine(ObjectDumper.Dump(output));

      var sendableMute = output.Cast<SendableMute>().Single();
      Assert.AreEqual("", sendableMute.Reason);
    }

    [TestMethod]
    public void RepositoryPunishmentFactory_FourthOffense_NoBanReason() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<ReceivedFactory>().PublicReceivedSnapshot(term);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      punishmentFactory.Create(snapshot);
      punishmentFactory.Create(snapshot);
      punishmentFactory.Create(snapshot);
      var output = punishmentFactory.Create(snapshot);

      var sendableMute = output.Cast<SendableMute>().Single();
      Assert.AreEqual("", sendableMute.Reason);
    }

  }
}
