using System.Linq;
using Bot.Main.Moderate;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
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

      var punishments = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsFalse(punishments.Any());
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

      var punishments = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsFalse(punishments.Any());
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

      var punishments = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsTrue(punishments.Any());
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

      var punishments = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsTrue(punishments.Any());
    }

    [TestMethod]
    public void SelfSpamPunishmentFactory_SameText_2mNick100YourPastText() {
      var nick = "nick";
      var text = "text";
      var container = new TestContainerManager().Container;
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<SelfSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text));

      var punishments = selfSpamPunishmentFactory.Create(snapshot);

      Assert.AreEqual("2m nick: 100% = your past text", punishments.OfType<SendableMute>().Single().Transmission.Reason);
    }

    [TestMethod]
    public void SelfSpamPunishmentFactory_SameText_2mNick83YourPastText() {
      var nick = "nick";
      var text1 = "text 1";
      var text2 = "text 2";
      var container = new TestContainerManager().Container;
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var selfSpamPunishmentFactory = container.GetInstance<SelfSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text1));
      snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text1));
      var snapshot = (ISnapshot<Civilian, PublicMessage>) snapshotFactory.Create(receivedFactory.PublicReceivedMessage(nick, text2));

      var punishments = selfSpamPunishmentFactory.Create(snapshot);

      Assert.AreEqual("2m nick: 83% = your past text", punishments.OfType<SendableMute>().Single().Transmission.Reason);
    }

  }
}
