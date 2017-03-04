using System;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Logic.Tests.Helper;
using Bot.Models.Sendable;
using Bot.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests_Nuke {

    private readonly ContextBuilder _messagesWithDifferentCasing = (ContextBuilder) new ContextBuilder()
      .RadiusIs("1:00:00")
      .SubsequentlySpacedBy(TimeSpan.FromMinutes(1))
      .PublicMessage("1")
      .TargetedMessage("MESSAGE")
      .TargetedMessage("message")
      .PublicMessage("4");

    [TestMethod]
    public void Nuke_MessagesWithDifferentCasing_IsCaseInsensitive() {
      var container = NukeHelper.GetContainer(_messagesWithDifferentCasing.NextTimestamp(), _messagesWithDifferentCasing.NukeBlastRadius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();

      var nukeResults = logic.Nuke(_messagesWithDifferentCasing.Build(), factory.ParsedNuke("!nuke10m message"));

      var nukedUsers = nukeResults.OfType<SendableMute>().Select(umb => umb.Target);
      _messagesWithDifferentCasing.VerifyTargeted(nukedUsers);
    }

    [TestMethod]
    public void Aegis_MessagesWithDifferentCasing_IsCaseInsensitive() {
      var container = NukeHelper.GetContainer(_messagesWithDifferentCasing.NextTimestamp(), _messagesWithDifferentCasing.NukeBlastRadius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();
      var nuke = factory.ModPublicReceivedMessage("!nuke10m message");
      var contextWithNuke = _messagesWithDifferentCasing.Build().Concat(nuke.Wrap()).ToList();

      var aegisResults = logic.Aegis(contextWithNuke);

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      _messagesWithDifferentCasing.VerifyTargeted(aegisedUsers);
    }

    private readonly ContextBuilder _messagesInAndOutOfRadius = (ContextBuilder) new ContextBuilder()
        .InsertAt("0:59:59.9999999").PublicMessage("message") //Out of nuke range
        .InsertAt("1:00:00.0000000").TargetedMessage("message")
        .BuildAt(" 1:05:00.0000000")
        .RadiusIs("0:05");

    [TestMethod]
    public void Nuke_MessagesInAndOutOfRadius_TargetsOnlyInRadius() {
      var container = NukeHelper.GetContainer(_messagesInAndOutOfRadius.BuiltAt, _messagesInAndOutOfRadius.NukeBlastRadius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();

      var nukeResults = logic.Nuke(_messagesInAndOutOfRadius.Build(), factory.ParsedNuke("!nuke10m message"));

      var nukedUsers = nukeResults.OfType<SendableMute>().Select(umb => umb.Target);
      _messagesInAndOutOfRadius.VerifyTargeted(nukedUsers);
    }

    [TestMethod]
    public void Aegis_MessagesInAndOutOfRadius_TargetsOnlyInRadius() {
      var container = NukeHelper.GetContainer(_messagesInAndOutOfRadius.BuiltAt, _messagesInAndOutOfRadius.NukeBlastRadius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();
      var nuke = factory.ModPublicReceivedMessage("!nuke10m message");
      var contextWithNuke = _messagesInAndOutOfRadius.Build().Concat(nuke.Wrap()).ToList();

      var aegisResults = logic.Aegis(contextWithNuke);

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      _messagesInAndOutOfRadius.VerifyTargeted(aegisedUsers);
    }

  }
}
