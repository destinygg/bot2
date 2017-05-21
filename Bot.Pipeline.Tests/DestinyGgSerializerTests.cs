using System;
using System.Collections.Generic;
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

    [TestMethod]
    public void DestinyGgSerializer_SingleMessage_IsNotModified() {
      var text = "and here, we... go";
      var container = new TestContainerManager().Container;
      var message = new SendablePublicMessage(text);
      var destinyGgSerializer = container.GetInstance<DestinyGgSerializer>();
      var expected = @"MSG {""Data"":""and here, we... go""}";

      var serialized = destinyGgSerializer.Create(message.Wrap().ToList());

      Assert.AreEqual(expected, serialized.Single());
    }

    [TestMethod]
    public void DestinyGgSerializer_TwoRepeatedMessages_AreMadeDistinct() {
      var text = "and here, we... go";
      var message = new SendablePublicMessage(text);
      var messages = new List<SendablePublicMessage> { message, message };
      var container = new TestContainerManager().Container;
      var destinyGgSerializer = container.GetInstance<DestinyGgSerializer>();
      var expected1 = @"MSG {""Data"":""and here, we... go""}";
      var expected2 = @"MSG {""Data"":""and here, we... go.""}";

      var serialized = destinyGgSerializer.Create(messages).ToList();

      Assert.AreEqual(2, serialized.Count);
      Assert.AreEqual(expected1, serialized.First());
      Assert.AreEqual(expected2, serialized.Skip(1).First());
    }

    [TestMethod]
    public void DestinyGgSerializer_TwoCallsSameMessage_AreMadeDistinct() {
      var text = "and here, we... go";
      var message = new SendablePublicMessage(text);
      var messages = new List<SendablePublicMessage> { message };
      var container = new TestContainerManager().Container;
      var destinyGgSerializer = container.GetInstance<DestinyGgSerializer>();
      var expected1 = @"MSG {""Data"":""and here, we... go""}";
      var expected2 = @"MSG {""Data"":""and here, we... go.""}";

      var serialized1 = destinyGgSerializer.Create(messages).ToList();
      var serialized2 = destinyGgSerializer.Create(messages).ToList();

      Assert.AreEqual(1, serialized1.Count);
      Assert.AreEqual(1, serialized2.Count);
      Assert.AreEqual(expected1, serialized1.Single());
      Assert.AreEqual(expected2, serialized2.Single());
    }

    [TestMethod]
    public void DestinyGgSerializer_ThreeRepeatedMessages_AreMadeAlternating() {
      var text = "and here, we... go";
      var message = new SendablePublicMessage(text);
      var messages = new List<SendablePublicMessage> { message, message, message };
      var container = new TestContainerManager().Container;
      var destinyGgSerializer = container.GetInstance<DestinyGgSerializer>();
      var expected1 = @"MSG {""Data"":""and here, we... go""}";
      var expected2 = @"MSG {""Data"":""and here, we... go.""}";
      var expected3 = @"MSG {""Data"":""and here, we... go""}";

      var serialized = destinyGgSerializer.Create(messages).ToList();

      Assert.AreEqual(3, serialized.Count);
      Assert.AreEqual(expected1, serialized.Skip(0).First());
      Assert.AreEqual(expected2, serialized.Skip(1).First());
      Assert.AreEqual(expected3, serialized.Skip(2).First());
    }

    [TestMethod]
    public void DestinyGgSerializer_ThreeSeparateRepeatedMessages_AreMadeAlternating() {
      var text = "and here, we... go";
      var message = new SendablePublicMessage(text);
      var messages = new List<SendablePublicMessage> { message };
      var container = new TestContainerManager().Container;
      var destinyGgSerializer = container.GetInstance<DestinyGgSerializer>();
      var expected1 = @"MSG {""Data"":""and here, we... go""}";
      var expected2 = @"MSG {""Data"":""and here, we... go.""}";
      var expected3 = @"MSG {""Data"":""and here, we... go""}";

      var serialized1 = destinyGgSerializer.Create(messages).ToList();
      var serialized2 = destinyGgSerializer.Create(messages).ToList();
      var serialized3 = destinyGgSerializer.Create(messages).ToList();

      Assert.AreEqual(1, serialized1.Count);
      Assert.AreEqual(1, serialized2.Count);
      Assert.AreEqual(1, serialized3.Count);
      Assert.AreEqual(expected1, serialized1.Single());
      Assert.AreEqual(expected2, serialized2.Single());
      Assert.AreEqual(expected3, serialized3.Single());
    }

    [TestMethod]
    public void DestinyGgSerializer_FourRepeatedMessages_AreMadeAlternating() {
      var text = "and here, we... go";
      var message = new SendablePublicMessage(text);
      var messages = new List<SendablePublicMessage> { message, message, message, message };
      var container = new TestContainerManager().Container;
      var destinyGgSerializer = container.GetInstance<DestinyGgSerializer>();
      var expected1 = @"MSG {""Data"":""and here, we... go""}";
      var expected2 = @"MSG {""Data"":""and here, we... go.""}";
      var expected3 = @"MSG {""Data"":""and here, we... go""}";
      var expected4 = @"MSG {""Data"":""and here, we... go.""}";

      var serialized = destinyGgSerializer.Create(messages).ToList();

      Assert.AreEqual(4, serialized.Count);
      Assert.AreEqual(expected1, serialized.Skip(0).First());
      Assert.AreEqual(expected2, serialized.Skip(1).First());
      Assert.AreEqual(expected3, serialized.Skip(2).First());
      Assert.AreEqual(expected4, serialized.Skip(3).First());
    }

  }
}
