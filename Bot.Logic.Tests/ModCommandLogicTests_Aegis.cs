using System;
using System.Linq;
using Bot.Models.Sendable;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests_Aegis {

    private ModCommandLogic _GetLogic(DateTime time) {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(time);
      return _GetLogic(timeService);
    }

    private ModCommandLogic _GetLogic(string time) {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(TestHelper.Parse(time));
      return _GetLogic(timeService);
    }

    private ModCommandLogic _GetLogic(ITimeService timeService) {
      var containerManager = new TestContainerManager(
        container => {
          var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, container);
          container.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
        });
      return containerManager.Container.GetInstance<ModCommandLogic>();
    }

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
      var logic = _GetLogic(contextBuilder.NextTimestamp());

      var aegis = logic.Aegis(context);

      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void OutOfRangeAegis() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt("0:59:59.9999999").PublicMessage("message")
        .InsertAt("1:00           ").TargetedMessage("message")
        .InsertAt("1:05           ").ModMessage("!nuke MESSage").Build();
      var time = " 1:05";
      var logic = _GetLogic(time);

      var aegis = logic.Aegis(context);

      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target);
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
      var logic = _GetLogic(contextBuilder.NextTimestamp());

      var aegis = logic.Aegis(context);

      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void RegexNukeAegisExtremeRange() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt("0:59:59.9999999").PublicMessage("abc")
        .InsertAt("1:00           ").PublicMessage("abc")
        .InsertAt("1:05           ").ModMessage("!nukeregex30m abc")
        .InsertAt("1:10           ").TargetedMessage("abc").Build();
      var time = " 1:40";
      var logic = _GetLogic(time);

      var aegis = logic.Aegis(context);

      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target); //oftykpe punishment?
      contextBuilder.VerifyTargeted(aegisedUsers);
    }

    [TestMethod]
    public void RegexNukeAegisOutOfRange() {
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .InsertAt("0:59:59.9999999").PublicMessage("abc")
        .InsertAt("1:00           ").PublicMessage("abc")
        .InsertAt("1:05           ").ModMessage("!nukeregex30m abc")
        .InsertAt("1:10           ").PublicMessage("abc").Build();
      var time = " 1:40:00.0000001";
      var logic = _GetLogic(time);

      var aegis = logic.Aegis(context);

      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target); //oftykpe punishment?
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
      var time = contextBuilder.NextTimestamp();
      var logic = _GetLogic(time);

      var aegis = logic.Aegis(context);

      var aegisedUsers = aegis.OfType<SendablePardon>().Select(umb => umb.Target); //oftykpe punishment?
      contextBuilder.VerifyTargeted(aegisedUsers);
    }

  }
}
