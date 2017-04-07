using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Bot.Database.Entities;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Repository.Tests;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class BanFactoryTests {

    private Container InitializeContainerAndRepository(string term, [CallerMemberName] string sqliteName = null) {
      var container = RepositoryHelper.GetContainerWithInitializedAndIsolatedRepository(
        sqliteName,
        settings => settings.MinimumPunishmentSimilarity.Returns(0.7d)
      );
      container.GetInstance<IQueryCommandService<IUnitOfWork>>().Command(db =>
        db.AutoPunishments.Add(new AutoPunishment(new AutoPunishmentEntity {
          Term = term,
          Type = AutoPunishmentType.MutedString,
          Duration = Convert.ToInt64(TimeSpan.FromMinutes(10).TotalSeconds),
        })));
      return container;
    }

    [TestMethod]
    public void BanFactory_NotInList_DoesNotPunish() {
      var bannedTerm = TestHelper.RandomString();
      var spokenTerm = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(bannedTerm);
      var snapshot = container.GetInstance<IReceivedFactory>().PublicReceivedSnapshot(spokenTerm);
      var banFactory = container.GetInstance<BanFactory>();

      var ban = banFactory.Create(snapshot);

      Assert.IsFalse(ban.Any());
    }

    [TestMethod]
    public void BanFactory_FirstOffense_10MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<IReceivedFactory>().PublicReceivedSnapshot(term);
      var banFactory = container.GetInstance<BanFactory>();

      var output = banFactory.Create(snapshot);

      var mute = output.Cast<SendableMute>().Single();
      Assert.IsTrue(mute.Duration.TotalMinutes == 10);
    }

    [TestMethod]
    public void BanFactory_SecondOffense_20MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<IReceivedFactory>().PublicReceivedSnapshot(term);
      var banFactory = container.GetInstance<BanFactory>();

      banFactory.Create(snapshot);
      var output = banFactory.Create(snapshot);

      var mute = output.Cast<SendableMute>().Single();
      Assert.AreEqual(20, mute.Duration.TotalMinutes);
    }

    [TestMethod]
    public void BanFactory_ThirdOffense_40MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<IReceivedFactory>().PublicReceivedSnapshot(term);
      var banFactory = container.GetInstance<BanFactory>();

      banFactory.Create(snapshot);
      banFactory.Create(snapshot);
      var output = banFactory.Create(snapshot);

      var mute = output.Cast<SendableMute>().Single();
      Assert.AreEqual(40, mute.Duration.TotalMinutes);
    }

    [TestMethod]
    public void BanFactory_FourthOffense_80MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = InitializeContainerAndRepository(term);
      var snapshot = container.GetInstance<IReceivedFactory>().PublicReceivedSnapshot(term);
      var banFactory = container.GetInstance<BanFactory>();

      banFactory.Create(snapshot);
      banFactory.Create(snapshot);
      banFactory.Create(snapshot);
      var output = banFactory.Create(snapshot);

      var mute = output.Cast<SendableMute>().Single();
      Assert.AreEqual(80, mute.Duration.TotalMinutes);
    }

  }
}
