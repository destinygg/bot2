using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class DestinyGgParserTests {

    [TestMethod]
    public void InitialUsers_Parses_WithoutError() {
      var data = TestData.DestinyGgClientNames;
      var container = new TestContainerManager();
      var parser = container.Container.GetInstance<DestinyGgParser>();
      var received = parser.Create(data);
    }

    [TestMethod]
    public void InitialUsers_ParsesIsPunishable_Properly() {
      var data = TestData.DestinyGgClientNames;
      var container = new TestContainerManager();
      var parser = container.Container.GetInstance<DestinyGgParser>();
      var received = parser.Create(data);
      var initialUsers = (InitialUsers) received.Transmission;
      var notPunishable = initialUsers.Users.Where(x => !x.IsPunishable).Select(x => x.Nick);
      Assert.IsTrue(notPunishable.SequenceEqual(new List<string> { "RightToBearArmsLOL", "CeneZa", "Destiny", "Bot", "woopboop" }));
    }

    [TestMethod]
    public void InitialUsers_ParsesMods_Properly() {
      var data = TestData.DestinyGgClientNames;
      var container = new TestContainerManager();
      var parser = container.Container.GetInstance<DestinyGgParser>();
      var received = parser.Create(data);
      var initialUsers = (InitialUsers) received.Transmission;
      var notPunishable = initialUsers.Users.Where(x => x.IsMod).Select(x => x.Nick);
      Assert.IsTrue(notPunishable.SequenceEqual(new List<string> { "RightToBearArmsLOL", "CeneZa", "Destiny", "Bot" }));
    }

    [TestMethod]
    public void InitialUsers_ParsesPublicMessages_WithoutError() {
      var data = TestData.DestinyGgPublicMsg;
      var container = new TestContainerManager();
      var parser = container.Container.GetInstance<DestinyGgParser>();
      var received = parser.Create(data);
    }

    [TestMethod]
    public void InitialUsers_ParsesPublicMessages_AsPunishable() {
      var data = TestData.DestinyGgPublicMsg;
      var container = new TestContainerManager();
      var parser = container.Container.GetInstance<DestinyGgParser>();
      var received = parser.Create(data);
      Assert.IsTrue(received.Sender.IsPunishable);
    }

    [TestMethod]
    public void InitialUsers_ParsesPublicMessagesFromProtected_AsNonpunishable() {
      var data = TestData.DestinyGgPublicMsgFromProtected;
      var container = new TestContainerManager();
      var parser = container.Container.GetInstance<DestinyGgParser>();
      var received = parser.Create(data);
      Assert.IsFalse(received.Sender.IsPunishable);
    }

    [TestMethod]
    public void InitialUsers_ParsesPublicMessagesFromMod_AsMod() {
      var data = TestData.DestinyGgPublicMsgFromMod;
      var container = new TestContainerManager();
      var parser = container.Container.GetInstance<DestinyGgParser>();
      var received = parser.Create(data);
      Assert.IsTrue(received.Sender.IsMod);
    }

    [TestMethod]
    public void InitialUsers_ParsesPublicMessages_WithCorrectTimestamp() {
      var data = TestData.DestinyGgPublicMsg;
      var container = new TestContainerManager();
      var parser = container.Container.GetInstance<DestinyGgParser>();
      var timestamp = parser.Create(data).Timestamp;
      Assert.AreEqual(new DateTime(2017, 5, 9, 0, 28, 35, 517), timestamp);
    }

    [TestMethod]
    public void InitialUsers_ParsesJoin_WithoutError() {
      var data = TestData.DestinyGgJoin;
      var container = new TestContainerManager();
      var parser = container.Container.GetInstance<DestinyGgParser>();
      var received = parser.Create(data);
    }

  }
}
