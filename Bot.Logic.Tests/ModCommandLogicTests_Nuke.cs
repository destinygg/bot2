using System;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests_Nuke {

    private Container _GetContainer(DateTime time) {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(time);
      return _GetContainer(timeService);
    }

    private Container _GetContainer(string time) {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(TimeParser.Parse(time));
      return _GetContainer(timeService);
    }

    private Container _GetContainer(ITimeService timeService) {
      var containerManager = new TestContainerManager(
        container => {
          var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, container);
          container.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
        });
      return containerManager.Container;
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
      var container = _GetContainer(contextBuilder.NextTimestamp());
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();

      var nuke = logic.Nuke(context, factory.ParsedNuke("!nuke10m message"));

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
      var container = _GetContainer(time);
      var logic = container.GetInstance<ModCommandLogic>();
      var factory = container.GetInstance<IReceivedFactory>();

      var nuke = logic.Nuke(context, factory.ParsedNuke("!nuke10m message"));

      var nukedUsers = nuke.OfType<SendableMute>().Select(umb => umb.Target);
      contextBuilder.VerifyTargeted(nukedUsers);
    }

  }
}
