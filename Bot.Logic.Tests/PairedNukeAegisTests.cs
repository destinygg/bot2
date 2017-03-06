using System;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Logic.Tests.Helper;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class PairedNukeAegisTests {

    private readonly ITerminalAppender _messagesWithDifferentCasing = new ContextAppenderBuilder(TimeSpan.FromMinutes(1))
      .PublicMessage("1")
      .TargetedMessage("MESSAGE")
      .TargetedMessage("message")
      .PublicMessage("4")
      .RadiusIs("1:00:00");

    [TestMethod]
    public void Nuke_MessagesWithDifferentCasing_IsCaseInsensitive() {
      var container = NukeHelper.GetContainer(_messagesWithDifferentCasing.NextTimestamp(), _messagesWithDifferentCasing.NukeBlastRadius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();
      var nukeFactory = container.GetInstance<IFactory<IReceived<Moderator, IMessage>, Nuke>>();

      var nukeResults = logic.Nuke(_messagesWithDifferentCasing.Build(), nukeFactory.Create(factory.ModPublicReceivedMessage("!nuke10m message")));

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

    private readonly ITerminalInserter _messagesInAndOutOfRadius = new ContextInserterBuilder()
      .InsertAt("0:59:59.9999999  ").PublicMessage("message") //Out of nuke range
      .InsertAt("1:00:00.0000000").TargetedMessage("message")
      .CreateAt(" 1:05:00.0000000")
      .RadiusIs("0:05");

    [TestMethod]
    public void Nuke_MessagesInAndOutOfRadius_TargetsOnlyInRadius() {
      var container = NukeHelper.GetContainer(_messagesInAndOutOfRadius.CreatedAt, _messagesInAndOutOfRadius.NukeBlastRadius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();
      var nukeFactory = container.GetInstance<IFactory<IReceived<Moderator, IMessage>, Nuke>>();

      var nukeResults = logic.Nuke(_messagesInAndOutOfRadius.Build(), nukeFactory.Create(factory.ModPublicReceivedMessage("!nuke10m message")));

      var nukedUsers = nukeResults.OfType<SendableMute>().Select(umb => umb.Target);
      _messagesInAndOutOfRadius.VerifyTargeted(nukedUsers);
    }

    [TestMethod]
    public void Aegis_MessagesInAndOutOfRadius_TargetsOnlyInRadius() {
      var container = NukeHelper.GetContainer(_messagesInAndOutOfRadius.CreatedAt, _messagesInAndOutOfRadius.NukeBlastRadius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();
      var nuke = factory.ModPublicReceivedMessage("!nuke10m message");
      var contextWithNuke = _messagesInAndOutOfRadius.Build().Concat(nuke.Wrap()).ToList();

      var aegisResults = logic.Aegis(contextWithNuke);

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      _messagesInAndOutOfRadius.VerifyTargeted(aegisedUsers);
    }

    private readonly ITerminalAppender _messagesContainingAndSimilarToNukedWord = new ContextAppenderBuilder(TimeSpan.FromTicks(1))
      .PublicMessage("1")
      .TargetedMessage("MESSAGE")
      .TargetedMessage("message")
      .TargetedMessage("the quick brown message jumped over the lazy dog")
      .PublicMessage("2")
      .RadiusIs("1:00:00");

    [TestMethod]
    public void Nuke_MessagesContainingAndSimilarToNukedWord_TargetsAppropriate() {
      var container = NukeHelper.GetContainer(_messagesContainingAndSimilarToNukedWord.NextTimestamp(), _messagesContainingAndSimilarToNukedWord.NukeBlastRadius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();
      var nukeFactory = container.GetInstance<IFactory<IReceived<Moderator, IMessage>, Nuke>>();

      var nukeResults = logic.Nuke(_messagesContainingAndSimilarToNukedWord.Build(), nukeFactory.Create(factory.ModPublicReceivedMessage("!nuke10m message")));

      var nukedUsers = nukeResults.OfType<SendableMute>().Select(umb => umb.Target);
      _messagesContainingAndSimilarToNukedWord.VerifyTargeted(nukedUsers);
    }

    [TestMethod]
    public void Aegis_MessagesContainingAndSimilarToNuked_TargetsAppropriate() {
      var container = NukeHelper.GetContainer(_messagesContainingAndSimilarToNukedWord.NextTimestamp(), _messagesContainingAndSimilarToNukedWord.NukeBlastRadius);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();
      var nuke = factory.ModPublicReceivedMessage("!nuke10m message");
      var contextWithNuke = _messagesContainingAndSimilarToNukedWord.Build().Concat(nuke.Wrap()).ToList();

      var aegisResults = logic.Aegis(contextWithNuke);

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      _messagesContainingAndSimilarToNukedWord.VerifyTargeted(aegisedUsers);
    }

  }
}
