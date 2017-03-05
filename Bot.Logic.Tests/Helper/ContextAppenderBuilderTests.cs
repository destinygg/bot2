using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests.Helper {
  [TestClass]
  public class ContextAppenderBuilderTests {

    [TestMethod]
    public void VerifyTargetedViaAppender_WithOneTarget_DoesNotThrowException() {
      var appender = new ContextAppenderBuilder(TimeSpan.FromHours(1))
        .PublicMessage()
        .TargetedMessage();

      appender.VerifyTargeted(new List<string> { "#2" }.Select(i => new Civilian(i)));
    }

    [TestMethod]
    public void VerifyTargetedViaAppender_WithTwoTargets_DoesNotThrowException() {
      var appender = new ContextAppenderBuilder(TimeSpan.FromHours(1))
        .PublicMessage()
        .TargetedMessage()
        .TargetedMessage();

      appender.VerifyTargeted(new List<string> { "#2", "#3" }.Select(i => new Civilian(i)));
    }

    [TestMethod]
    public void VerifyTargetedViaAppender_WithTwoTargetsSwapped_DoesNotThrowException() {
      var appender = new ContextAppenderBuilder(TimeSpan.FromHours(1))
        .PublicMessage()
        .TargetedMessage()
        .TargetedMessage();

      appender.VerifyTargeted(new List<string> { "#3", "#2" }.Select(i => new Civilian(i)));
    }

    [TestMethod]
    public void VerifyTargetedViaAppender_WithTwoTargetsButOneMissing_ThrowsException() {
      var contextBuilder = new ContextAppenderBuilder(TimeSpan.FromHours(1))
        .PublicMessage()
        .TargetedMessage()
        .TargetedMessage();

      var exception = TestHelper.AssertCatch<Exception>(
        () => contextBuilder.VerifyTargeted(new List<string> { "#2" }.Select(i => new Civilian(i))));
      Assert.AreEqual("Expected targets are not equal to actual targets.", exception.Message);
    }

    [TestMethod]
    public void VerifyTargetedViaAppender_WithTwoTargetsButOneTooMany_ThrowsException() {
      var contextBuilder = new ContextAppenderBuilder(TimeSpan.FromHours(1))
        .PublicMessage()
        .TargetedMessage()
        .TargetedMessage();

      var exception = TestHelper.AssertCatch<Exception>(
        () => contextBuilder.VerifyTargeted(new List<string> { "#1", "#2", "#3" }.Select(i => new Civilian(i))));
      Assert.AreEqual("Expected targets are not equal to actual targets.", exception.Message);
    }

    [TestMethod]
    public void SubsequentlySpacedBy_1HourWith3Messages_AreAt123Hours() {
      var context = new ContextAppenderBuilder(TimeSpan.FromHours(1))
        .PublicMessage()
        .TargetedMessage()
        .ModMessage().Build();

      Assert.AreEqual(DateTimeZero.AddHours(1), context[0].Timestamp);
      Assert.AreEqual(DateTimeZero.AddHours(2), context[1].Timestamp);
      Assert.AreEqual(DateTimeZero.AddHours(3), context[2].Timestamp);
    }

    [TestMethod]
    public void NextTimestamp_With3MessagesSpacedBy1Hour_Yields4Hours() {
      var appender = new ContextAppenderBuilder(TimeSpan.FromHours(1))
        .PublicMessage()
        .TargetedMessage()
        .ModMessage();

      var nextTimestamp = appender.NextTimestamp();

      Assert.AreEqual(DateTimeZero.AddHours(4), nextTimestamp);
    }

    [TestMethod]
    public void NextTimestamp_WithIntervalOfZero_ThrowsException() {
      var contextBuilder = new ContextAppenderBuilder(TimeSpan.Zero);

      var exception = TestHelper.AssertCatch<ArgumentOutOfRangeException>(() => contextBuilder.NextTimestamp());

      Assert.IsTrue(exception.Message.Contains("Interval is less than or equal to zero."));
    }

  }
}
