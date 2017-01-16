﻿using System;
using System.Linq;
using Bot.Models;
using Bot.Pipeline.Contracts;
using Bot.Tools;
using Bot.Tools.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests_Aegis {
    private ModCommandLogic _GetLogic() {
      var logger = new Mock<ILogger>().Object;
      var timeService = new Mock<ITimeService>().Object;
      var regex = new ModCommandRegex();
      var parser = new ModCommandParser(regex, logger);
      return new ModCommandLogic(logger, regex, parser, timeService);
    }

    [TestMethod]
    public void SimpleAegis() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .PublicMessage("derp")
        .TargetedMessage("MESSAGE")
        .TargetedMessage("message")
        .TargetedMessage("message the quick brown fox jumped over the lazy dog")
        .PublicMessage("Innocent as well")
        .ModMessage("!nuke MESSage")
        .GetContext;

      var logic = _GetLogic();

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.Cast<SendableUnMuteBan>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

    [TestMethod]
    public void OutOfRangeAegis() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .PublicMessage("message") //Out of aegis range
        .TargetedMessage("message", Settings.AegisRadiusAroundFirstNuke.Add(TimeSpan.FromTicks(1)))
        .ModMessage("!nuke MESSage")
        .GetContext;

      var logic = _GetLogic();

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.Cast<SendableUnMuteBan>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

    [TestMethod]
    public void DoubleNukeAegis() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .PublicMessage("innocent")
        .TargetedMessage("xyz")
        .TargetedMessage("abc")
        .PublicMessage("herp")
        .ModMessage("!nuke xyz")
        .ModMessage("!nuke abc")
        .PublicMessage("derp")
        .GetContext;

      var logic = _GetLogic();

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.Cast<SendableUnMuteBan>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

    [TestMethod]
    public void DoubleRegexNukeAegis() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .PublicMessage("innocent")
        .TargetedMessage("xyz")
        .TargetedMessage("abc")
        .PublicMessage("herp")
        .ModMessage("!nukeregex xyz")
        .ModMessage("!nukeregex abc")
        .PublicMessage("derp")
        .GetContext;

      var logic = _GetLogic();

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.Cast<SendableUnMuteBan>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

    [TestMethod]
    public void DoubleNukeRegexNukeAegis() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .PublicMessage("innocent")
        .TargetedMessage("xyz")
        .TargetedMessage("abc")
        .PublicMessage("herp")
        .ModMessage("!nuke xyz")
        .ModMessage("!nukeregex abc")
        .PublicMessage("derp")
        .GetContext;

      var logic = _GetLogic();

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.Cast<SendableUnMuteBan>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

  }
}