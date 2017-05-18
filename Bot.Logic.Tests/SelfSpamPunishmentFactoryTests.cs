using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class SelfSpamPunishmentFactoryTests {

    [TestMethod]
    public void SelfSpamPunishmentFactory_x1_DoesNotPunish() {
      var nick = "nick";
      var text = "text";
      var container = new TestContainerManager().Container;
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<SelfSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsFalse(bans.Any());
    }

    [TestMethod]
    public void SelfSpamPunishmentFactory_x2_DoesNotPunish() {
      var nick = "nick";
      var text = "text";
      var container = new TestContainerManager().Container;
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<SelfSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsFalse(bans.Any());
    }

    [TestMethod]
    public void SelfSpamPunishmentFactory_x3_Punishes() {
      var nick = "nick";
      var text = "text";
      var container = new TestContainerManager().Container;
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<SelfSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsTrue(bans.Any());
    }

    [TestMethod]
    public void SelfSpamPunishmentFactory_x4_Punishes() {
      var nick = "nick";
      var text = "text";
      var container = new TestContainerManager().Container;
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<SelfSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsTrue(bans.Any());
    }

  }
}
