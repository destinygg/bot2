using System;
using System.Linq;
using Bot.Models;
using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests_Nuke {
    private IReceivedFactory _factory;
    private ModCommandLogic _GetLogic(ContextBuilder contextBuilder) {
      var logger = new Mock<ILogger>().Object;
      var timeServiceMock = new Mock<ITimeService>();
      timeServiceMock.Setup(ts => ts.UtcNow).Returns(contextBuilder.GetTimestampOfZerothReceived);
      var timeService = timeServiceMock.Object;
      var regex = new ModCommandRegex();
      var parser = new ModCommandParser(regex, logger);
      _factory = new ReceivedFactory(timeService, parser);
      var nukeLogic = new NukeLogic(regex, _factory);
      return new ModCommandLogic(logger, nukeLogic);
    }

    [TestMethod]
    public void SimpleNuke() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .PublicMessage("derp")
        .TargetedMessage("MESSAGE")
        .TargetedMessage("message")
        .TargetedMessage("message the quick brown fox jumped over the lazy dog")
        .PublicMessage("Innocent as well")
        .GetContext;

      var logic = _GetLogic(contextBuilder);

      //Act
      var nuke = logic.Nuke(context, _factory.ReceivedStringNuke("!nuke10m message"));

      //Assert
      var nukedUsers = nuke.OfType<SendableMute>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(nukedUsers));
    }

    [TestMethod]
    public void OutOfRangeNuke() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .PublicMessage("message", TimeSpan.Zero - TimeSpan.FromTicks(1)) //Out of nuke range
        .TargetedMessage("message", TimeSpan.Zero) //Inside nuke range
        .TargetedMessage("message", Settings.NukeBlastRadius)
        .PublicMessage("innocent", Settings.NukeBlastRadius + TimeSpan.FromMinutes(1))
        .SetTimestampOfZerothReceived(Settings.NukeBlastRadius)
        .GetContext;

      var logic = _GetLogic(contextBuilder);

      //Act
      var nuke = logic.Nuke(context, _factory.ReceivedStringNuke("!nuke10m message"));

      //Assert
      var nukedUsers = nuke.OfType<SendableMute>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(nukedUsers));
    }

  }
}
