using System;
using System.Linq;
using Bot.Models.Sendable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests_Aegis {

    [TestMethod]
    public void SimpleAegis() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .SubsequentlySpacedBy(TimeSpan.FromTicks(1))
        .PublicMessage("derp")
        .TargetedMessage("MESSAGE")
        .TargetedMessage("message")
        .TargetedMessage("message the quick brown fox jumped over the lazy dog")
        .PublicMessage("Innocent as well")
        .ModMessage("!nuke MESSage").Build();
      var nukeBlastRadius = TimeSpan.FromMinutes(100);
      var container = NukeHelper.GetContainer(contextBuilder.NextTimestamp(), nukeBlastRadius);

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(context);

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void OutOfRangeAegis() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt(" 0:59:59.9999999").PublicMessage("message")
        .InsertAt(" 1:00           ").TargetedMessage("message")
        .InsertAt(" 1:05           ").ModMessage("!nuke MESSage").Build();
      var time = "  1:05";
      var radius = "0:05";
      var container = NukeHelper.GetContainer(time, radius);

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(context);

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void DoubleNukeAegis() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .SubsequentlySpacedBy(TimeSpan.FromTicks(1))
        .PublicMessage("innocent")
        .TargetedMessage("xyz")
        .TargetedMessage("abc")
        .PublicMessage("herp")
        .ModMessage("!nuke xyz")
        .ModMessage("!nuke abc")
        .PublicMessage("derp")
        .TargetedMessage("xyz")
        .TargetedMessage("abc").Build();
      var container = NukeHelper.GetContainer(contextBuilder.NextTimestamp(), "0:05");

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(context);

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void RegexNukeAegisExtremeRange() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt(" 0:59:59.9999999").PublicMessage("abc")
        .InsertAt(" 1:00           ").PublicMessage("abc")
        .InsertAt(" 1:05           ").ModMessage("!nukeregex30m abc")
        .InsertAt(" 1:10           ").TargetedMessage("abc").Build();
      var radius = "0:05";
      var time = "  1:40";
      var container = NukeHelper.GetContainer(time, radius);

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(context);

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void RegexNukeAegisOutOfRange() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt(" 0:59:59.9999999").PublicMessage("abc")
        .InsertAt(" 1:00           ").PublicMessage("abc")
        .InsertAt(" 1:05           ").ModMessage("!nukeregex30m abc")
        .InsertAt(" 1:10           ").PublicMessage("abc").Build();
      var time = "  1:40:00.0000001";
      var radius = "0:05";
      var container = NukeHelper.GetContainer(time, radius);

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(context);

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void DoubleNukeRegexNukeAegis() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .SubsequentlySpacedBy(TimeSpan.FromTicks(1))
        .PublicMessage("innocent")
        .TargetedMessage("xyz")
        .TargetedMessage("abc")
        .PublicMessage("herp")
        .ModMessage("!nuke xyz")
        .ModMessage("!nukeregex abc")
        .PublicMessage("derp").Build();
      var container = NukeHelper.GetContainer(contextBuilder.NextTimestamp(), "0:05");

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(context);

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(aegisedUsers);
    }

  }
}
