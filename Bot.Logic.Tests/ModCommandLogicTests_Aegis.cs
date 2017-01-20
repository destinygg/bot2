using System;
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
    private ModCommandLogic _GetLogic(ContextBuilder contextBuilder) {
      var logger = new Mock<ILogger>().Object;
      var timeServiceMock = new Mock<ITimeService>();
      timeServiceMock.Setup(ts => ts.UtcNow).Returns(contextBuilder.GetTimestampOfZerothReceived);
      var timeService = timeServiceMock.Object;
      var regex = new ModCommandRegex();
      var parser = new ModCommandParser(regex, logger);
      var factory = new ReceivedFactory(timeService, parser);
      var nukeLogic = new NukeLogic(regex, factory, timeService);
      return new ModCommandLogic(logger, nukeLogic);
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

      var logic = _GetLogic(contextBuilder);

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

    [TestMethod]
    public void OutOfRangeAegis() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var nukeTime = TimeSpan.FromMinutes(10);
      var outOfAegisRange = nukeTime - Settings.NukeBlastRadius - TimeSpan.FromTicks(1);
      var insideAegisRange = nukeTime - Settings.NukeBlastRadius;

      var context = contextBuilder
        .PublicMessage("message", outOfAegisRange) //Out of aegis range
        .TargetedMessage("message", insideAegisRange)
        .ModMessage("!nuke MESSage", nukeTime)
        .GetContext;

      var logic = _GetLogic(contextBuilder);

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target).ToList();
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
        .TargetedMessage("xyz")
        .TargetedMessage("abc")
        .GetContext;

      var logic = _GetLogic(contextBuilder);

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

    [TestMethod]
    public void RegexNukeAegisExtremeRange() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .PublicMessage("abc", TimeSpan.FromTicks(-1))
        .PublicMessage("abc", TimeSpan.Zero)
        .ModMessage("!nukeregex30m abc", Settings.NukeBlastRadius)
        .TargetedMessage("abc", Settings.NukeBlastRadius.Multiply(2))
        .SetTimestampOfZerothReceived(Settings.NukeBlastRadius.Multiply(2) + TimeSpan.FromMinutes(30))
        .GetContext;

      var logic = _GetLogic(contextBuilder);

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

    [TestMethod]
    public void RegexNukeAegisOutOfRange() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .PublicMessage("abc", TimeSpan.FromTicks(-1))
        .PublicMessage("abc", TimeSpan.Zero)
        .ModMessage("!nukeregex30m abc", Settings.NukeBlastRadius)
        .PublicMessage("abc", Settings.NukeBlastRadius.Multiply(2))
        .SetTimestampOfZerothReceived(Settings.NukeBlastRadius.Multiply(2) + TimeSpan.FromMinutes(30) + TimeSpan.FromTicks(1))
        .GetContext;

      var logic = _GetLogic(contextBuilder);

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target).ToList();
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

      var logic = _GetLogic(contextBuilder);

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

  }
}
