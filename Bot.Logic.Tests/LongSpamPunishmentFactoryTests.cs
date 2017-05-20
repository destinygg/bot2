using System;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class LongSpamPunishmentFactoryTests {

    private Container GetContainer(int longSpamMinimumLength) => new TestContainerManager(configureSettings: s => s.LongSpamMinimumLength = longSpamMinimumLength).Container;

    [TestMethod]
    public void LongSpamPunishmentFactory_x1_DoesNotPunish() {
      var nick = "nick";
      var longSpamMinimumLength = 60;
      var text = TestHelper.RandomString(longSpamMinimumLength);
      var container = GetContainer(longSpamMinimumLength);
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<LongSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsFalse(bans.Any());
    }

    [TestMethod]
    public void LongSpamPunishmentFactory_x2_Punishes() {
      var nick = "nick";
      var longSpamMinimumLength = 60;
      var text = TestHelper.RandomString(longSpamMinimumLength);
      var container = GetContainer(longSpamMinimumLength);
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<LongSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsTrue(bans.Any());
    }

    [TestMethod]
    public void LongSpamPunishmentFactory_MinimumMessageLength_1mMute() {
      var nick = "nick";
      var longSpamMinimumLength = 60;
      var text = TestHelper.RandomString(longSpamMinimumLength);
      var container = GetContainer(longSpamMinimumLength);
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<LongSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      var banDuration = bans.OfType<SendableMute>().Single().Duration;
      Assert.AreEqual(TimeSpan.FromMinutes(1), banDuration);
    }

    [TestMethod]
    public void LongSpamPunishmentFactory_2xMinimumMessageLength_7mMute() {
      var nick = "nick";
      var longSpamMinimumLength = 60;
      var text = TestHelper.RandomString(longSpamMinimumLength * 2);
      var container = GetContainer(longSpamMinimumLength);
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<LongSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      var banDuration = bans.OfType<SendableMute>().Single().Duration;
      Assert.AreEqual(TimeSpan.FromMinutes(7), banDuration);
    }

    [TestMethod]
    public void LongSpamPunishmentFactory_3xMinimumMessageLength_13mMute() {
      var nick = "nick";
      var longSpamMinimumLength = 60;
      var text = TestHelper.RandomString(longSpamMinimumLength * 3);
      var container = GetContainer(longSpamMinimumLength);
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<LongSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      var banDuration = bans.OfType<SendableMute>().Single().Duration;
      Assert.AreEqual(TimeSpan.FromMinutes(13), banDuration);
    }

    [TestMethod]
    public void LongSpamPunishmentFactory_SameMessageMinimumLength_1mNick100PastText() {
      var nick = "nick";
      var longSpamMinimumLength = 60;
      var text = TestHelper.RandomString(longSpamMinimumLength);
      var container = GetContainer(longSpamMinimumLength);
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<LongSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      var reason = bans.OfType<SendableMute>().Single().Reason;
      Assert.AreEqual("1m nick: 100% = past text", reason);
    }

    [TestMethod]
    public void LongSpamPunishmentFactory_SimilarMessageMinimumLength_7mNick94PastText() {
      var nick = "nick";
      var longSpamMinimumLength = 60;
      var text = TestHelper.RandomString(longSpamMinimumLength);
      var container = GetContainer(longSpamMinimumLength);
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<LongSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text + text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      var reason = bans.OfType<SendableMute>().Single().Reason;
      Assert.AreEqual("7m nick: 94% = past text", reason);
    }

    [TestMethod]
    public void LongSpamPunishmentFactory_SameMessageMinimumLengthWithNonSpam_1mNick100PastText() {
      var nick = "nick";
      var longSpamMinimumLength = 60;
      var x2SpamText = TestHelper.RandomString(longSpamMinimumLength);
      var otherText = TestHelper.RandomString(longSpamMinimumLength);
      var container = GetContainer(longSpamMinimumLength);
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<LongSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();

      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, otherText));
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, x2SpamText));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, x2SpamText));
      var bans = selfSpamPunishmentFactory.Create(snapshot);

      var reason = bans.OfType<SendableMute>().Single().Reason;
      Assert.AreEqual("1m nick: 100% = past text", reason);
    }

  }
}
