using System;
using System.Linq;
using Bot.Logic.Tests.Helper;
using Bot.Models.Sendable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests_Aegis {

    [TestMethod]
    public void OutOfRangeAegis() {
      var inserter = new ContextInserterBuilder()
        .InsertAt(" 0:59:59.9999999  ").PublicMessage("message")
        .InsertAt(" 1:00           ").TargetedMessage("message")
        .InsertAt(" 1:05                ").ModMessage("!nuke MESSage")
        .CreateAt(" 1:05")
        .RadiusIs(" 0:05");
      var container = NukeHelper.GetContainer(inserter.CreatedAt, inserter.NukeBlastRadius);

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(inserter.Build());

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      inserter.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void DoubleNukeAegis() {
      var appender = new ContextAppenderBuilder(TimeSpan.FromTicks(1))
        .PublicMessage("innocent")
        .TargetedMessage("xyz")
        .TargetedMessage("abc")
        .PublicMessage("herp")
        .ModMessage("!nuke xyz")
        .ModMessage("!nuke abc")
        .PublicMessage("derp")
        .TargetedMessage("xyz")
        .TargetedMessage("abc")
        .RadiusIs("0:05");
      var container = NukeHelper.GetContainer(appender.NextTimestamp(), appender.NukeBlastRadius);

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(appender.Build());

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      appender.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void RegexNukeAegisExtremeRange() {
      var inserter = new ContextInserterBuilder()
        .InsertAt("0:59:59.9999999").PublicMessage("abc")
        .InsertAt("1:00           ").PublicMessage("abc")
        .InsertAt("1:05           ").ModMessage("!nukeregex30m abc")
        .InsertAt("1:10           ").TargetedMessage("abc")
        .CreateAt("1:40")
        .RadiusIs("0:05");
      var container = NukeHelper.GetContainer(inserter.CreatedAt, inserter.NukeBlastRadius);

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(inserter.Build());

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      inserter.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void RegexNukeAegisOutOfRange() {
      var inserter = new ContextInserterBuilder()
        .InsertAt("0:59:59.9999999").PublicMessage("abc")
        .InsertAt("1:00           ").PublicMessage("abc")
        .InsertAt("1:05           ").ModMessage("!nukeregex30m abc")
        .InsertAt("1:10           ").PublicMessage("abc")
        .CreateAt("1:40:00.0000001")
        .RadiusIs("0:05");
      var container = NukeHelper.GetContainer(inserter.CreatedAt, inserter.NukeBlastRadius);

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(inserter.Build());

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      inserter.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void DoubleNukeRegexNukeAegis() {
      var appender = new ContextAppenderBuilder(TimeSpan.FromTicks(1))
        .PublicMessage("innocent")
        .TargetedMessage("xyz")
        .TargetedMessage("abc")
        .PublicMessage("herp")
        .ModMessage("!nuke xyz")
        .ModMessage("!nukeregex abc")
        .PublicMessage("derp")
        .RadiusIs("0:05");
      var container = NukeHelper.GetContainer(appender.NextTimestamp(), appender.NukeBlastRadius);

      var aegisResults = container.GetInstance<ModCommandLogic>().Aegis(appender.Build());

      var aegisedUsers = aegisResults.OfType<SendablePardon>().Select(umb => umb.Target);
      appender.VerifyTargeted(aegisedUsers);
    }

  }
}
