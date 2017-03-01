using System;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests_Nuke {

    [TestMethod]
    public void SimpleNuke() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .SubsequentlySpacedBy(TimeSpan.FromTicks(1))
        .PublicMessage("1")
        .TargetedMessage("MESSAGE")
        .TargetedMessage("message")
        .PublicMessage("4").Build();
      var nukeBlastRadius = TimeSpan.FromMinutes(100);
      var container = NukeHelper.GetContainer(contextBuilder.NextTimestamp(), nukeBlastRadius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();

      var nukeResults = logic.Nuke(context, factory.ParsedNuke("!nuke10m message"));

      var nukedUsers = nukeResults.OfType<SendableMute>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(nukedUsers);
    }

    [TestMethod]
    public void OutOfRangeNuke() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt(" 0:59:59.9999999").PublicMessage("message") //Out of nuke range
        .InsertAt(" 1:00:00.0000000").TargetedMessage("message").Build();//Inside nuke range
      var time = "  1:05:00.0000000";
      var radius = "0:05";
      var container = NukeHelper.GetContainer(time, radius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();

      var nukeResults = logic.Nuke(context, factory.ParsedNuke("!nuke10m message"));

      var nukedUsers = nukeResults.OfType<SendableMute>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(nukedUsers);
    }

  }
}
