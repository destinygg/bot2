using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Tests;
using Bot.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests.Helper {
  [TestClass]
  public class ContextInserterBuilderTests {

    [TestMethod]
    public void MessagesAt0_00_00_0000001_AreAlways_At1Tick() {
      var anyDateTime = "0";

      var timestamp = new ContextInserterBuilder().InsertAt("0:00:00.0000001").PublicMessage().CreateAt(anyDateTime).Build().Single().Timestamp;

      Assert.AreEqual(timestamp, DateTimeZero.AddTicks(1));
    }

    [TestMethod]
    public void MessagesAt_0_00_00_0000001__AreAlways_At1Tick() {
      var anyDateTime = "0";

      var timestamp = new ContextInserterBuilder().InsertAt(" 0:00:00.0000001 ").PublicMessage().CreateAt(anyDateTime).Build().Single().Timestamp;

      Assert.AreEqual(timestamp, DateTimeZero.AddTicks(1));
    }

    [TestMethod]
    public void MessagesAt01_0000001_AreAlways_At1SecondAnd1Tick() {
      var anyDateTime = "0";

      var timestamp = new ContextInserterBuilder().InsertAt("0:00:01.0000001").PublicMessage().CreateAt(anyDateTime).Build().Single().Timestamp;

      Assert.AreEqual(timestamp, DateTimeZero.AddSeconds(1).AddTicks(1));
    }

    [TestMethod]
    public void MessagesAt00_00_01_AreAlways_At1Second() {
      var anyDateTime = "0";

      var timestamp = new ContextInserterBuilder().InsertAt("00:00:01").PublicMessage().CreateAt(anyDateTime).Build().Single().Timestamp;

      Assert.AreEqual(timestamp, DateTimeZero.AddSeconds(1));
    }

    [TestMethod]
    public void MessagesAt0_04_AreAlways_At4Minutes() {
      var anyDateTime = "0";

      var timestamp = new ContextInserterBuilder().InsertAt("0:04").PublicMessage().CreateAt(anyDateTime).Build().Single().Timestamp;

      Assert.AreEqual(timestamp, DateTimeZero.AddMinutes(4));
    }

    [TestMethod]
    public void MessagesAt0_34_AreAlways_At00_34() {
      var anyDateTime = "0";

      var timestamp = new ContextInserterBuilder().InsertAt("0:34").PublicMessage().CreateAt(anyDateTime).Build().Single().Timestamp;

      Assert.AreEqual(timestamp, DateTimeZero.AddMinutes(34));
    }

    [TestMethod]
    public void MessagesAt2_34_AreAlways_At2_34() {
      var anyDateTime = "0";

      var timestamp = new ContextInserterBuilder().InsertAt("2:34").PublicMessage().CreateAt(anyDateTime).Build().Single().Timestamp;

      Assert.AreEqual(timestamp, DateTimeZero.AddHours(2).AddMinutes(34));
    }

    [TestMethod]
    public void MessagesAt_2_34_AreAlways_At02_34() {
      var anyDateTime = "0";

      var timestamp = new ContextInserterBuilder().InsertAt(" 2:34").PublicMessage().CreateAt(anyDateTime).Build().Single().Timestamp;

      Assert.AreEqual(timestamp, DateTimeZero.AddHours(2).AddMinutes(34));
    }

    [TestMethod]
    public void MessagesAt12_34_AreAlways_At12_34() {
      var anyDateTime = "0";

      var timestamp = new ContextInserterBuilder().InsertAt("12:34").PublicMessage().CreateAt(anyDateTime).Build().Single().Timestamp;

      Assert.AreEqual(timestamp, DateTimeZero.AddHours(12).AddMinutes(34));
    }

    [TestMethod]
    public void MessagesAt00_00_00_0000001_AreAlways_At1Tick() {
      var anyDateTime = "0";

      var timestamp = new ContextInserterBuilder().InsertAt("00:00:00.0000001").PublicMessage().CreateAt(anyDateTime).Build().Single().Timestamp;

      Assert.AreEqual(timestamp, DateTimeZero.AddTicks(1));
    }

    [TestMethod]
    public void AddingTwoMessages_WithTheSameTimestamp_ThrowsException() {
      var anyDateTime = "0";

      var exception = TestHelper.AssertCatch<Exception>(
        () => new ContextInserterBuilder().InsertAt("1").PublicMessage().InsertAt("1").TargetedMessage().CreateAt(anyDateTime));

      Assert.AreEqual("Nicks/timestamps must be unique. If you want messages with the same timestamp, zero pad them.", exception.Message);
    }

    [TestMethod]
    public void AddingTwoMessages_WithVirtuallyTheSameTimestampButOneIsZeroPadded_DoesNotThrowException() {
      var anyDateTime = "0";

      new ContextInserterBuilder().InsertAt("1").PublicMessage().InsertAt("01").TargetedMessage().CreateAt(anyDateTime).Build();
    }

    [TestMethod]
    public void VerifyTargeted_WithOneTarget_DoesNotThrowException() {
      var anyDateTime = "0";

      var inserter = new ContextInserterBuilder()
        .InsertAt("1").PublicMessage()
        .InsertAt("2").TargetedMessage().CreateAt(anyDateTime);

      inserter.VerifyTargeted("2".Wrap().Select(i => new Civilian(i)));
    }

    [TestMethod]
    public void VerifyTargeted_WithTwoTargets_DoesNotThrowException() {
      var anyDateTime = "0";

      var inserter = new ContextInserterBuilder()
        .InsertAt("1").PublicMessage()
        .InsertAt("2").TargetedMessage()
        .InsertAt("3").TargetedMessage().CreateAt(anyDateTime);

      inserter.VerifyTargeted(new List<string> { "2", "3" }.Select(i => new Civilian(i)));
    }

    [TestMethod]
    public void VerifyTargeted_WithTwoTargetsSwapped_DoesNotThrowException() {
      var anyDateTime = "0";

      var inserter = new ContextInserterBuilder()
        .InsertAt("1").PublicMessage()
        .InsertAt("2").TargetedMessage()
        .InsertAt("3").TargetedMessage().CreateAt(anyDateTime);

      inserter.VerifyTargeted(new List<string> { "3", "2" }.Select(i => new Civilian(i)));
    }

    [TestMethod]
    public void VerifyTargeted_WithTwoTargetsButOneMissing_ThrowsException() {
      var anyDateTime = "0";

      var inserter = new ContextInserterBuilder()
        .InsertAt("1").PublicMessage()
        .InsertAt("2").TargetedMessage()
        .InsertAt("3").TargetedMessage().CreateAt(anyDateTime);

      var exception = TestHelper.AssertCatch<Exception>(
        () => inserter.VerifyTargeted(new List<string> { "2" }.Select(i => new Civilian(i))));
      Assert.AreEqual("Expected targets are not equal to actual targets.", exception.Message);
    }

    [TestMethod]
    public void VerifyTargeted_WithTwoTargetsButOneTooMany_ThrowsException() {
      var anyDateTime = "0";

      var inserter = new ContextInserterBuilder()
        .InsertAt("1").PublicMessage()
        .InsertAt("2").TargetedMessage()
        .InsertAt("3").TargetedMessage().CreateAt(anyDateTime);

      var exception = TestHelper.AssertCatch<Exception>(
        () => inserter.VerifyTargeted(new List<string> { "1", "2", "3" }.Select(i => new Civilian(i))));
      Assert.AreEqual("Expected targets are not equal to actual targets.", exception.Message);
    }

    [TestMethod]
    public void RadiusIs_1_00_Yields1Hour() {
      var inserter = new ContextInserterBuilder()
        .InsertAt("0").ModMessage()
        .CreateAt("0")
        .RadiusIs("1:00");

      Assert.AreEqual(TimeSpan.FromHours(1), inserter.NukeBlastRadius);
    }

    [TestMethod]
    public void CreatedAt_1_00_IsCreatedAt1hour() {
      var inserter = new ContextInserterBuilder()
        .InsertAt("0").ModMessage()
        .CreateAt("1:00");

      Assert.AreEqual(DateTimeZero.AddHours(1), inserter.CreatedAt);
    }

    [TestMethod]
    public void NukeBlastRadius_IsUnassigned_ThrowsNullException() {
      var inserter = new ContextInserterBuilder();

      var exception = TestHelper.AssertCatch<InvalidOperationException>(() => inserter.NukeBlastRadius);

      Assert.AreEqual("Nullable object must have a value.", exception.Message);
    }

  }
}
