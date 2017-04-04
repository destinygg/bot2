using System;
using System.Linq;
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

namespace Bot.Logic.Tests {
  [TestClass]
  public class BanFactoryTests {
    [TestMethod]
    public void BanFactory_NotInList_DoesNotPunish() {
      var bannedTerm = TestHelper.RandomString();
      var spokenTerm = TestHelper.RandomString();
      var container = RepositoryHelper.GetContainerWithInitializedAndIsolatedRepository(
        configureSettings: settings => settings.MinimumPunishmentSimilarity.Returns(0.7d)
      );
      container.GetInstance<IQueryCommandService<IUnitOfWork>>().Command(db =>
        db.AutoPunishments.Add(new AutoPunishment(new AutoPunishmentEntity {
          Term = bannedTerm,
          Type = AutoPunishmentType.MutedString,
          Duration = Convert.ToInt64(TimeSpan.FromMinutes(10).TotalSeconds),
        })));
      var snapshot = container.GetInstance<IReceivedFactory>().PublicReceivedSnapshot(spokenTerm);
      var banFactory = container.GetInstance<BanFactory>();

      var ban = banFactory.Create(snapshot);

      Assert.IsFalse(ban.Any());
    }

    [TestMethod]
    public void BanFactory_FirstOffense_10MinutePunishment() {
      var term = TestHelper.RandomString();
      var container = RepositoryHelper.GetContainerWithInitializedAndIsolatedRepository(
        configureSettings: settings => settings.MinimumPunishmentSimilarity.Returns(0.7d)
      );
      container.GetInstance<IQueryCommandService<IUnitOfWork>>().Command(db =>
        db.AutoPunishments.Add(new AutoPunishment(new AutoPunishmentEntity {
          Term = term,
          Type = AutoPunishmentType.MutedString,
          Duration = Convert.ToInt64(TimeSpan.FromMinutes(10).TotalSeconds),
        })));
      var snapshot = container.GetInstance<IReceivedFactory>().PublicReceivedSnapshot(term);
      var banFactory = container.GetInstance<BanFactory>();

      var output = banFactory.Create(snapshot);

      var mute = output.Cast<SendableMute>().Single();
      Assert.IsTrue(mute.Duration.TotalMinutes == 10);
    }

  }
}
