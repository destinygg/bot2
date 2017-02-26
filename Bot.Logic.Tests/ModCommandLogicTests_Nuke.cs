using System;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests_Nuke {
    private IReceivedFactory _factory;
    private ModCommandLogic _GetLogic(DateTime time) {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(time);
      return ModCommandLogic(timeService);
    }

    private ModCommandLogic _GetLogic(string time) {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(TimeParser.Parse(time));
      return ModCommandLogic(timeService);
    }

    private ModCommandLogic ModCommandLogic(ITimeService timeService) {
      ILogger logger = null;
      var regex = new ModCommandRegex();
      var parser = new ModCommandParser(regex, logger);
      _factory = new ReceivedFactory(timeService, parser, regex, logger);
      var nukeLogic = new NukeLogic(regex, _factory);
      return new ModCommandLogic(logger, nukeLogic);
    }

    [TestMethod]
    public void SimpleNuke() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .SubsequentlySpacedBy(TimeSpan.FromTicks(1))
        .PublicMessage("derp")
        .TargetedMessage("MESSAGE")
        .TargetedMessage("message")
        .TargetedMessage("message the quick brown fox jumped over the lazy dog")
        .PublicMessage("Innocent as well").Build();
      var logic = _GetLogic(contextBuilder.NextTimestamp);

      var nuke = logic.Nuke(context, _factory.ParsedNuke("!nuke10m message"));

      var nukedUsers = nuke.OfType<SendableMute>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(nukedUsers);
    }

    [TestMethod]
    public void OutOfRangeNuke() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt("0:59:59.9999999").PublicMessage("message") //Out of nuke range
        .InsertAt("1:00:00.0000000").TargetedMessage("message").Build();//Inside nuke range
      var time = " 1:05:00.0000000";
      var logic = _GetLogic(time);

      var nuke = logic.Nuke(context, _factory.ParsedNuke("!nuke10m message"));

      var nukedUsers = nuke.OfType<SendableMute>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(nukedUsers);
    }

  }
}
