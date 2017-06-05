using System.Linq;
using Bot.Main.Moderate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class SingleLineSpamPunishmentFactoryTests {

    [TestMethod]
    public void SingleLineSpamPunishmentFactory_MinimumRepeat_DoesNotPunish() {
      var minimumRepeatLength = 3;
      var text = "aaa";
      var container = new TestContainerManager(configureSettings: s => s.RepeatCharacterSpamLimit = minimumRepeatLength).Container;
      var selfSpamPunishmentFactory = container.GetInstance<SingleLineSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      var snapshot = receivedFactory.PublicReceivedSnapshot(text);

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsFalse(bans.Any());
    }

    [TestMethod]
    public void SingleLineSpamPunishmentFactory_MinimumRepeatPlus1_Punishes() {
      var minimumRepeatLength = 3;
      var text = "aaa" + "a";
      var container = new TestContainerManager(configureSettings: s => s.RepeatCharacterSpamLimit = minimumRepeatLength).Container;
      var selfSpamPunishmentFactory = container.GetInstance<SingleLineSpamPunishmentFactory>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      var snapshot = receivedFactory.PublicReceivedSnapshot(text);

      var bans = selfSpamPunishmentFactory.Create(snapshot);

      Assert.IsTrue(bans.Any());
    }

  }
}
