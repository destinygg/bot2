using System;
using System.Linq;
using Bot.Models;
using Bot.Models.Sendable;
using Bot.Tests;
using Bot.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class DestinyGgSerializerTests {

    [TestMethod]
    public void DestinyGgSerializer_BanMaxDuration_ParsesProperly() {
      var container = new TestContainerManager().Container;
      var sendableBan = new SendableBan(new Civilian("User"), TimeSpan.MaxValue);
      var destinyGgSerializer = container.GetInstance<DestinyGgSerializer>();
      var expected = @"BAN {""Nick"":""User"",""IsPermanent"":true,""Reason"":""Unspecified reason""}";

      var serialized = destinyGgSerializer.Create(sendableBan.Wrap().ToList());

      Assert.AreEqual(expected, serialized.Single());
    }

    [TestMethod]
    public void DestinyGgSerializer_BanSomeDuration_ParsesProperly() {
      var container = new TestContainerManager().Container;
      var sendableBan = new SendableBan(new Civilian("User"), TimeSpan.FromSeconds(1));
      var destinyGgSerializer = container.GetInstance<DestinyGgSerializer>();
      var expected = @"BAN {""Nick"":""User"",""Duration"":1000000000,""Reason"":""Unspecified reason""}";

      var serialized = destinyGgSerializer.Create(sendableBan.Wrap().ToList());

      Assert.AreEqual(expected, serialized.Single());
    }

    [TestMethod]
    public void DestinyGgSerializer_IpbanMaxDuration_ParsesProperly() {
      var container = new TestContainerManager().Container;
      var sendableBan = new SendableIpban(new Civilian("User"), TimeSpan.MaxValue);
      var destinyGgSerializer = container.GetInstance<DestinyGgSerializer>();
      var expected = @"BAN {""Nick"":""User"",""BanIp"":true,""IsPermanent"":true,""Reason"":""Unspecified reason""}";

      var serialized = destinyGgSerializer.Create(sendableBan.Wrap().ToList());

      Assert.AreEqual(expected, serialized.Single());
    }

    [TestMethod]
    public void DestinyGgSerializer_IpbanSomeDuration_ParsesProperly() {
      var container = new TestContainerManager().Container;
      var sendableBan = new SendableIpban(new Civilian("User"), TimeSpan.FromSeconds(1));
      var destinyGgSerializer = container.GetInstance<DestinyGgSerializer>();
      var expected = @"BAN {""Nick"":""User"",""Duration"":1000000000,""BanIp"":true,""Reason"":""Unspecified reason""}";

      var serialized = destinyGgSerializer.Create(sendableBan.Wrap().ToList());

      Assert.AreEqual(expected, serialized.Single());
    }

  }
}
