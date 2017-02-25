﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Database.Tests;
using Bot.Models;
using Bot.Models.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ContextBuilderTests {

    [TestMethod]
    public void MessagesAt0_0000001_AreAlways_At1Tick() {
      var context = new ContextBuilder().InsertAt("0.0000001").PublicMessage().Build();

      var timestamp = context.Single().Timestamp;
      Assert.AreEqual(timestamp, DateTime.MinValue + TimeSpan.FromTicks(1));
    }

    [TestMethod]
    public void MessagesAt00_0000001_AreAlways_At1Tick() {
      var context = new ContextBuilder().InsertAt("00.0000001").PublicMessage().Build();

      var timestamp = context.Single().Timestamp;
      Assert.AreEqual(timestamp, DateTime.MinValue + TimeSpan.FromTicks(1));
    }

    [TestMethod]
    public void MessagesAt01_0000001_AreAlways_At1SecondAnd1Tick() {
      var context = new ContextBuilder().InsertAt("01.0000001").PublicMessage().Build();

      var timestamp = context.Single().Timestamp;
      Assert.AreEqual(timestamp, DateTime.MinValue + TimeSpan.FromSeconds(1) + TimeSpan.FromTicks(1));
    }

    [TestMethod]
    public void MessagesAt00_00_01_AreAlways_At1Second() {
      var context = new ContextBuilder().InsertAt("00:00:01").PublicMessage().Build();

      var timestamp = context.Single().Timestamp;
      Assert.AreEqual(timestamp, DateTime.MinValue + TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    public void MessagesAt4_AreAlways_At4Minutes() {
      var context = new ContextBuilder().InsertAt("4").PublicMessage().Build();

      var timestamp = context.Single().Timestamp;
      Assert.AreEqual(timestamp, DateTime.MinValue + TimeSpan.FromMinutes(4));
    }

    [TestMethod]
    public void MessagesAt34_AreAlways_At00_34() {
      var context = new ContextBuilder().InsertAt("34").PublicMessage().Build();

      var timestamp = context.Single().Timestamp;
      Assert.AreEqual(timestamp, DateTime.MinValue + TimeSpan.FromMinutes(34));
    }

    [TestMethod]
    public void MessagesAt2_34_AreAlways_At02_34() {
      var context = new ContextBuilder().InsertAt("2:34").PublicMessage().Build();

      var timestamp = context.Single().Timestamp;
      Assert.AreEqual(timestamp, DateTime.MinValue + TimeSpan.FromHours(2) + TimeSpan.FromMinutes(34));
    }

    [TestMethod]
    public void MessagesAt_2_34_AreAlways_At02_34() {
      var context = new ContextBuilder().InsertAt(" 2:34").PublicMessage().Build();

      var timestamp = context.Single().Timestamp;
      Assert.AreEqual(timestamp, DateTime.MinValue + TimeSpan.FromHours(2) + TimeSpan.FromMinutes(34));
    }

    [TestMethod]
    public void MessagesAt12_34_AreAlways_At12_34() {
      var context = new ContextBuilder().InsertAt("12:34").PublicMessage().Build();

      var timestamp = context.Single().Timestamp;
      Assert.AreEqual(timestamp, DateTime.MinValue + TimeSpan.FromHours(12) + TimeSpan.FromMinutes(34));
    }

    [TestMethod]
    public void MessagesAt00_00_00_0000001_AreAlways_At1Tick() {
      var context = new ContextBuilder().InsertAt("00:00:00.0000001").PublicMessage().Build();

      var timestamp = context.Single().Timestamp;
      Assert.AreEqual(timestamp, DateTime.MinValue + TimeSpan.FromTicks(1));
    }

    [TestMethod]
    public void AddingTwoMessages_WithTheSameTimestamp_ThrowsException() {
      var exception = TestHelper.AssertCatch<Exception>(
        () => new ContextBuilder().InsertAt("1").PublicMessage().InsertAt("1").TargetedMessage().Build());
      Assert.AreEqual("Nicks/timestamps must be unique. If you want messages with the same timestamp, zero pad them.", exception.Message);
    }

    [TestMethod]
    public void AddingTwoMessages_WithVirtuallyTheSameTimestampButOneIsZeroPadded_DoesNotThrowException() {
      new ContextBuilder().InsertAt("1").PublicMessage().InsertAt("01").TargetedMessage().Build();
    }

    [TestMethod]
    public void VerifyTargeted_WithOneTarget_DoesNotThrowException() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt("1").PublicMessage()
        .InsertAt("2").TargetedMessage().Build();

      contextBuilder.VerifyTargeted(new List<string> { "2" }.Select(i => new Civilian(i)));
    }

    [TestMethod]
    public void VerifyTargeted_WithTwoTargets_DoesNotThrowException() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt("1").PublicMessage()
        .InsertAt("2").TargetedMessage()
        .InsertAt("3").TargetedMessage().Build();

      contextBuilder.VerifyTargeted(new List<string> { "2", "3" }.Select(i => new Civilian(i)));
    }

    [TestMethod]
    public void VerifyTargeted_WithTwoTargetsSwapped_DoesNotThrowException() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt("1").PublicMessage()
        .InsertAt("2").TargetedMessage()
        .InsertAt("3").TargetedMessage().Build();

      contextBuilder.VerifyTargeted(new List<string> { "3", "2" }.Select(i => new Civilian(i)));
    }

    [TestMethod]
    public void VerifyTargeted_WithTwoTargetsButOneMissing_ThrowsException() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt("1").PublicMessage()
        .InsertAt("2").TargetedMessage()
        .InsertAt("3").TargetedMessage().Build();

      var exception = TestHelper.AssertCatch<Exception>(
        () => contextBuilder.VerifyTargeted(new List<string> { "2" }.Select(i => new Civilian(i))));
      Assert.AreEqual("Expected targets are not equal to actual targets.", exception.Message);
    }

    [TestMethod]
    public void VerifyTargeted_WithTwoTargetsButOneTooMany_ThrowsException() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt("1").PublicMessage()
        .InsertAt("2").TargetedMessage()
        .InsertAt("3").TargetedMessage().Build();

      var exception = TestHelper.AssertCatch<Exception>(
        () => contextBuilder.VerifyTargeted(new List<string> { "1", "2", "3" }.Select(i => new Civilian(i))));
      Assert.AreEqual("Expected targets are not equal to actual targets.", exception.Message);
    }

  }
}
