using System;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Main.Moderate;
using Bot.Models;
using Bot.Models.Json;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class LiveCommandTests {

    private readonly TwitchStreamStatus.RootObject _off = new TwitchStreamStatus.RootObject();
    private readonly TwitchStreamStatus.RootObject _on = new TwitchStreamStatus.RootObject {
      stream = new TwitchStreamStatus.Stream {
        created_at = "2017-04-24T21:23:16Z",
        game = "Sc2",
        channel = new TwitchStreamStatus.Channel {
          status = "Infestiny"
        }
      }
    };

    private Container _getContainer(DateTime now, DateTime onTime, DateTime offTime, TwitchStreamStatus.RootObject current, StreamStatus previous) {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(now);
      var urlJsonParser = Substitute.For<IGenericClassFactory<string, string, string>>();
      urlJsonParser.Create<TwitchStreamStatus.RootObject>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(current);
      var container = new TestContainerManager(c => {
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, c);
        c.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
        var urlJsonParserRegistration = Lifestyle.Singleton.CreateRegistration(() => urlJsonParser, c);
        c.RegisterConditional(typeof(IGenericClassFactory<string, string, string>), urlJsonParserRegistration, _ => true);
      }, s => s.OnOffTimeTolerance = TimeSpan.FromMinutes(2))
        .InitializeAndIsolateRepository();
      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      unitOfWork.Command(u => {
        u.StateIntegers.StreamStatus = previous;
        u.StateIntegers.LatestStreamOnTime = onTime;
        u.StateIntegers.LatestStreamOffTime = offTime;
      });
      return container;
    }

    [TestMethod]
    public void LiveCommand_OnToOff_StreamWentOfflineInThePast2mAndItsDurationWas3h() {
      var now = DateTime.Today.AddHours(1);
      var onTime = DateTime.Today.AddHours(-2);
      var offTime = DateTime.Today;
      var previousState = StreamStatus.On;
      var currentState = _off;
      var container = _getContainer(now, onTime, offTime, currentState, previousState);
      var commandLogic = container.GetInstance<ICommandLogic>();

      var message = commandLogic.Live().OfType<SendablePublicMessage>().Single();

      Assert.AreEqual("Stream went offline in the past ~2m and its duration was 3h", message.Text);
    }

    [TestMethod]
    public void LiveCommand_OnToOn1Hr_LiveWith0ViewersFor1hPlayingSc2EntitledInfestiny() {
      var now = DateTime.MinValue.AddHours(1);
      var onTime = DateTime.MinValue;
      var offTime = DateTime.MinValue;
      var previousState = StreamStatus.On;
      var currentState = _on;
      var container = _getContainer(now, onTime, offTime, currentState, previousState);
      var commandLogic = container.GetInstance<ICommandLogic>();

      var message = commandLogic.Live().OfType<SendablePublicMessage>().Single();

      Assert.AreEqual("Live with 0 viewers for 1h playing Sc2: Infestiny", message.Text);
    }

    [TestMethod]
    public void LiveCommand_OffToOff1Hr_StreamWentOffline1hAgoAndItsDurationWas2h() {
      var now = DateTime.Today.AddHours(1);
      var onTime = DateTime.Today.AddHours(-2);
      var offTime = DateTime.Today;
      var previousState = StreamStatus.Off;
      var currentState = _off;
      var container = _getContainer(now, onTime, offTime, currentState, previousState);
      var commandLogic = container.GetInstance<ICommandLogic>();

      var message = commandLogic.Live().OfType<SendablePublicMessage>().Single();

      Assert.AreEqual("Stream went offline 1h ago and its duration was 2h", message.Text);
    }

    [TestMethod]
    public void LiveCommand_OffToOn_LiveWith0ViewersFor0mPlayingSc2EntitledInfestiny() {
      var now = DateTime.MinValue;
      var onTime = DateTime.MinValue;
      var offTime = DateTime.MinValue;
      var previousState = StreamStatus.On;
      var currentState = _on;
      var container = _getContainer(now, onTime, offTime, currentState, previousState);
      var commandLogic = container.GetInstance<ICommandLogic>();

      var message = commandLogic.Live().OfType<SendablePublicMessage>().Single();

      Assert.AreEqual("Live with 0 viewers for 0m playing Sc2: Infestiny", message.Text);
    }

    [TestMethod]
    public void LiveCommand_PossiblyOffToOffWithin_StreamWentOffline1hAgoAndItsDurationWas2h() {
      var now = DateTime.Today.AddHours(1);
      var onTime = DateTime.Today.AddHours(-2);
      var offTime = DateTime.Today;
      var previousState = StreamStatus.PossiblyOff;
      var currentState = _off;
      var container = _getContainer(now, onTime, offTime, currentState, previousState);
      var commandLogic = container.GetInstance<ICommandLogic>();

      var message = commandLogic.Live().OfType<SendablePublicMessage>().Single();

      Assert.AreEqual("Stream went offline 1h ago and its duration was 2h", message.Text);
    }

    [TestMethod]
    public void LiveCommand_PossiblyOffToOff1hr_StreamWentOffline1hAgoAndItsDurationWas2h() {
      var now = DateTime.Today.AddHours(1);
      var onTime = DateTime.Today.AddHours(-2);
      var offTime = DateTime.Today;
      var previousState = StreamStatus.PossiblyOff;
      var currentState = _off;
      var container = _getContainer(now, onTime, offTime, currentState, previousState);
      var commandLogic = container.GetInstance<ICommandLogic>();

      var message = commandLogic.Live().OfType<SendablePublicMessage>().Single();

      Assert.AreEqual("Stream went offline 1h ago and its duration was 2h", message.Text);
    }

    [TestMethod]
    public void LiveCommand_PossiblyOffToOn1hr_LiveWith0ViewersFor1hPlayingSc2EntitledInfestiny() {
      var now = DateTime.MinValue.AddHours(1);
      var onTime = DateTime.MinValue;
      var offTime = DateTime.MinValue;
      var previousState = StreamStatus.PossiblyOff;
      var currentState = _on;
      var container = _getContainer(now, onTime, offTime, currentState, previousState);
      var commandLogic = container.GetInstance<ICommandLogic>();

      var message = commandLogic.Live().OfType<SendablePublicMessage>().Single();

      Assert.AreEqual("Live with 0 viewers for 1h playing Sc2: Infestiny", message.Text);
    }

  }
}
