using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logic.Interfaces;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Tests;
using Bot.Repository.Interfaces;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Main.Moderate.Tests {
  [TestClass]
  public class PeriodicTwitterStatusUpdaterTests {

    private static Container GetContainer(TestableSerializer sender) => new TestContainerManager(c => {
      var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, c);
      c.RegisterConditional(typeof(IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>), senderRegistration, _ => true);
    }, settings => settings.TwitterStatusUpdaterInterval = TimeSpan.FromMilliseconds(100))
      .InitializeAndIsolateRepository();

    [TestMethod]
    public void PeriodicTwitterStatusUpdaterHandle_NewDb_UpdatesDbAndSendsOneMessage_DoNotRunContinuously() {
      var sender = new TestableSerializer();
      var container = GetContainer(sender);
      var periodicTaskRunner = container.GetInstance<PeriodicTwitterStatusUpdater>();
      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var latestDestinyTweetId = unitOfWork.Query(u => u.StateIntegers.LatestDestinyTweetId);
      Assert.AreEqual(-1, latestDestinyTweetId);

      periodicTaskRunner.Handle();

      Task.Delay(400).Wait();
      Assert.AreEqual(1, sender.Outbox.Cast<SendablePublicMessage>().Count());
      latestDestinyTweetId = unitOfWork.Query(u => u.StateIntegers.LatestDestinyTweetId);
      Assert.AreNotEqual(-1, latestDestinyTweetId);
    }

    [TestMethod]
    public void PeriodicTwitterStatusUpdaterHandle_UpdatedDb_DoesntUpdateDbAndDoesntSendMessage_DoNotRunContinuously() {
      var sender = new TestableSerializer();
      var container = GetContainer(sender);
      var periodicTaskRunner = container.GetInstance<PeriodicTwitterStatusUpdater>();
      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var twitterManager = container.GetInstance<ITwitterManager>();
      var latestDestinyTweetId = unitOfWork.Query(u => u.StateIntegers.LatestDestinyTweetId);
      Assert.AreEqual(-1, latestDestinyTweetId);
      twitterManager.LatestTweetFromDestiny("");
      latestDestinyTweetId = unitOfWork.Query(u => u.StateIntegers.LatestDestinyTweetId);
      Assert.AreNotEqual(-1, latestDestinyTweetId);

      periodicTaskRunner.Handle();

      Task.Delay(400).Wait();
      Assert.AreEqual(0, sender.Outbox.Cast<SendablePublicMessage>().Count());
      latestDestinyTweetId = unitOfWork.Query(u => u.StateIntegers.LatestDestinyTweetId);
      Assert.AreNotEqual(-1, latestDestinyTweetId);
    }

  }
}
