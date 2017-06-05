using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Main.Moderate;
using Bot.Models.Sendable;
using Bot.Tests;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests {
    private TestContainerManager CreateTestContainerManager(string data) {
      var downloadFactory = Substitute.For<IFactory<string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>()).Returns(data);
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(new DateTime(2017, 1, 2, 0, 0, 0));
      return new TestContainerManager(c => {
        c.RegisterConditional<IGenericClassFactory<string, string, string>, UrlJsonParser>(Lifestyle.Singleton, _ => true);
        var downloadFactoryRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IFactory<string, string, string>), downloadFactoryRegistration, _ => true);
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, c);
        c.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
      }, s => s.ClientType = "DestinyGg");
    }

    [TestMethod]
    public void ModCommandLogic_OverRustleLogs_Returns3Lines() {
      var data = TestData.OverrustleLogs4Lines;
      var testContainerManager = CreateTestContainerManager(data).Container;
      var modCommandLogic = testContainerManager.GetInstance<IModCommandLogic>();
      var expected = new List<string> {
        "dgg.overrustlelogs.net/woopboop",
        "23h 58m ago: Some words 2017-01-01 00:02:00 UTC third to last",
        "23h ago: Some words 2017-01-01 01:00:00 UTC penultimate",
        "22h ago: Some words 2017-01-01 02:00:00 UTC last"
      };

      var logs = modCommandLogic.Stalk("woopboop");

      var text = logs.Cast<SendablePublicMessage>().Select(l => l.Transmission.Text);
      Assert.IsTrue(expected.SequenceEqual(text));
    }

    [TestMethod]
    public void ModCommandLogic_OverRustleLogsTwoLogs_Returns2Lines() {
      var data = TestData.OverrustleLogs2Lines;
      var testContainerManager = CreateTestContainerManager(data).Container;
      var modCommandLogic = testContainerManager.GetInstance<IModCommandLogic>();
      var expected = new List<string> {
        "dgg.overrustlelogs.net/woopboop",
        "23h ago: Some words 2017-01-01 01:00:00 UTC penultimate",
        "22h ago: Some words 2017-01-01 02:00:00 UTC last"
      };

      var logs = modCommandLogic.Stalk("woopboop");

      var text = logs.Cast<SendablePublicMessage>().Select(l => l.Transmission.Text);
      Assert.IsTrue(expected.SequenceEqual(text));
    }

    [TestMethod]
    public void ModCommandLogic_OverRustleLogs1Log_Returns1Line() {
      var data = TestData.OverrustleLogs1Line;
      var testContainerManager = CreateTestContainerManager(data).Container;
      var modCommandLogic = testContainerManager.GetInstance<IModCommandLogic>();
      var expected = new List<string> {
        "dgg.overrustlelogs.net/woopboop",
        "22h ago: Some words 2017-01-01 02:00:00 UTC last"
      };

      var logs = modCommandLogic.Stalk("woopboop");

      var text = logs.Cast<SendablePublicMessage>().Select(l => l.Transmission.Text);
      Console.WriteLine(ObjectDumper.Dump(text));
      Assert.IsTrue(expected.SequenceEqual(text));
    }

    [TestMethod]
    public void ModCommandLogic_OverRustleLogs404_ReturnsUserNotFound_DoNotRunContinuously() {
      var user = TestHelper.RandomString();
      var testContainerManager = new TestContainerManager(configureSettings: s => s.ClientType = "DestinyGg");
      var modCommandLogic = testContainerManager.Container.GetInstance<IModCommandLogic>();
      var expected = new List<string> {
        $"{user} not found",
      };

      var logs = modCommandLogic.Stalk(user);

      var text = logs.Cast<SendablePublicMessage>().Select(l => l.Transmission.Text);
      Console.WriteLine(ObjectDumper.Dump(text));
      Assert.IsTrue(expected.SequenceEqual(text));
    }

  }
}
