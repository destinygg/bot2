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

  }
}
