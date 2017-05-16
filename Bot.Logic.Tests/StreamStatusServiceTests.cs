using System;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Json;
using Bot.Repository.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class StreamStatusServiceTests {

    private readonly TwitchStreamStatus.RootObject _off = new TwitchStreamStatus.RootObject();
    private readonly TwitchStreamStatus.RootObject _on = new TwitchStreamStatus.RootObject {
      stream = new TwitchStreamStatus.Stream()
    };

    [TestMethod]
    public void StreamStatusService_StreamNotNull_IsOn() {
      var urlJsonParser = Substitute.For<IGenericClassFactory<string, string, string>>();
      urlJsonParser.Create<TwitchStreamStatus.RootObject>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(_on);
      var container = new TestContainerManager(c => {
        var urlJsonParserRegistration = Lifestyle.Singleton.CreateRegistration(() => urlJsonParser, c);
        c.RegisterConditional(typeof(IGenericClassFactory<string, string, string>), urlJsonParserRegistration, _ => true);
      }).InitializeAndIsolateRepository();
      var streamStatusService = container.GetInstance<IStreamStatusService>();

      var status = streamStatusService.Get();

      Assert.AreEqual(StreamStatus.On, status);
    }

    [TestMethod]
    public void StreamStatusService_StreamNull_IsOffline() {
      var urlJsonParser = Substitute.For<IGenericClassFactory<string, string, string>>();
      urlJsonParser.Create<TwitchStreamStatus.RootObject>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(_off);
      var container = new TestContainerManager(c => {
        var urlJsonParserRegistration = Lifestyle.Singleton.CreateRegistration(() => urlJsonParser, c);
        c.RegisterConditional(typeof(IGenericClassFactory<string, string, string>), urlJsonParserRegistration, _ => true);
      }).InitializeAndIsolateRepository();
      var streamStatusService = container.GetInstance<IStreamStatusService>();

      var status = streamStatusService.Get();

      Assert.AreEqual(StreamStatus.Off, status);
    }

    private readonly DateTime _time0 = DateTime.Today;
    private readonly DateTime _hour1 = DateTime.Today + TimeSpan.FromHours(1);
    private readonly DateTime _hour2 = DateTime.Today + TimeSpan.FromHours(2);
    private readonly DateTime _hour3 = DateTime.Today + TimeSpan.FromHours(3);
    private readonly DateTime _minu1 = DateTime.Today + TimeSpan.FromMinutes(1);
    private readonly DateTime _minu2 = DateTime.Today + TimeSpan.FromMinutes(2);
    private readonly DateTime _minu3 = DateTime.Today + TimeSpan.FromMinutes(3);
    private readonly DateTime _minu4 = DateTime.Today + TimeSpan.FromMinutes(4);
    private readonly DateTime _minu5 = DateTime.Today + TimeSpan.FromMinutes(5);

    private Container _getContainer(ITimeService timeService, IGenericClassFactory<string, string, string> urlJsonParser) {
      var container = new TestContainerManager(c => {
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, c);
        c.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
        var urlJsonParserRegistration = Lifestyle.Singleton.CreateRegistration(() => urlJsonParser, c);
        c.RegisterConditional(typeof(IGenericClassFactory<string, string, string>), urlJsonParserRegistration, _ => true);
      }, s => s.OnOffTimeTolerance = TimeSpan.FromMinutes(2))
        .InitializeAndIsolateRepository();
      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      unitOfWork.Command(u => {
        u.StateIntegers.StreamStatus = StreamStatus.Off;
        u.StateIntegers.LatestStreamOffTime = _time0;
      });
      return container;
    }

    [TestMethod]
    public void StreamStatusService_OffOnHourLater_UpdatesDatabase() {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(_hour1);
      var urlJsonParser = Substitute.For<IGenericClassFactory<string, string, string>>();
      urlJsonParser.Create<TwitchStreamStatus.RootObject>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(_off, _on);
      var container = _getContainer(timeService, urlJsonParser);
      var streamStatusService = container.GetInstance<IStreamStatusService>();

      Assert.AreEqual(StreamStatus.Off, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.On, streamStatusService.Get());

      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var onTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOnTime);
      Assert.AreEqual(_hour1, onTime);
      var status = unitOfWork.Query(u => u.StateIntegers.StreamStatus);
      Assert.AreEqual(StreamStatus.On, status);
    }

    [TestMethod]
    public void StreamStatusService_OffOnOffHourLater_UpdatesDatabase() {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(_hour1, _hour2);
      var urlJsonParser = Substitute.For<IGenericClassFactory<string, string, string>>();
      urlJsonParser.Create<TwitchStreamStatus.RootObject>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(_off, _on, _off);
      var container = _getContainer(timeService, urlJsonParser);
      var streamStatusService = container.GetInstance<IStreamStatusService>();

      Assert.AreEqual(StreamStatus.Off, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.On, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.PossiblyOff, streamStatusService.Get());

      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var onTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOnTime);
      Assert.AreEqual(_hour1, onTime);
      var offTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOffTime);
      Assert.AreEqual(_hour2, offTime);
      var currentState = unitOfWork.Query(u => u.StateIntegers.StreamStatus);
      Assert.AreEqual(StreamStatus.PossiblyOff, currentState);
    }

    [TestMethod]
    public void StreamStatusService_OffOnOffOffHourLater_UpdatesDatabase() {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(_hour1, _hour2, _hour3);
      var urlJsonParser = Substitute.For<IGenericClassFactory<string, string, string>>();
      urlJsonParser.Create<TwitchStreamStatus.RootObject>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(_off, _on, _off, _off);
      var container = _getContainer(timeService, urlJsonParser);
      var streamStatusService = container.GetInstance<IStreamStatusService>();

      Assert.AreEqual(StreamStatus.Off, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.On, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.PossiblyOff, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.Off, streamStatusService.Get());

      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var onTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOnTime);
      Assert.AreEqual(_hour1, onTime);
      var possibleStreamOffTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOffTime);
      Assert.AreEqual(_hour2, possibleStreamOffTime);
      var currentState = unitOfWork.Query(u => u.StateIntegers.StreamStatus);
      Assert.AreEqual(StreamStatus.Off, currentState);
    }

    [TestMethod]
    public void StreamStatusService_OffOnOffOffOffMinuteLater_UpdatesDatabase() {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(_minu1, _minu2, _minu3, _minu4);
      var urlJsonParser = Substitute.For<IGenericClassFactory<string, string, string>>();
      urlJsonParser.Create<TwitchStreamStatus.RootObject>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(_off, _on, _off, _off, _off);
      var container = _getContainer(timeService, urlJsonParser);
      var streamStatusService = container.GetInstance<IStreamStatusService>();

      Assert.AreEqual(StreamStatus.Off, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.On, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.PossiblyOff, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.PossiblyOff, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.Off, streamStatusService.Get());

      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var onTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOnTime);
      Assert.AreEqual(_minu1, onTime);
      var offTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOffTime);
      Assert.AreEqual(_minu2, offTime);
      var streamStatus = unitOfWork.Query(u => u.StateIntegers.StreamStatus);
      Assert.AreEqual(StreamStatus.Off, streamStatus);
    }

    [TestMethod]
    public void StreamStatusService_OffOnOffOffOffOnMinuteLater_UpdatesDatabase() {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(_minu1, _minu2, _minu3, _minu4, _minu5);
      var urlJsonParser = Substitute.For<IGenericClassFactory<string, string, string>>();
      urlJsonParser.Create<TwitchStreamStatus.RootObject>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(_off, _on, _off, _off, _off, _on);
      var container = _getContainer(timeService, urlJsonParser);
      var streamStatusService = container.GetInstance<IStreamStatusService>();

      Assert.AreEqual(StreamStatus.Off, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.On, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.PossiblyOff, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.PossiblyOff, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.Off, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.On, streamStatusService.Get());

      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var onTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOnTime);
      Assert.AreEqual(_minu5, onTime);
      var offTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOffTime);
      Assert.AreEqual(_minu2, offTime);
      var currentState = unitOfWork.Query(u => u.StateIntegers.StreamStatus);
      Assert.AreEqual(StreamStatus.On, currentState);
    }

    [TestMethod]
    public void StreamStatusService_OffOnOffOnMinuteLater_UpdatesDatabase() {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(_minu1, _minu2, _minu3, _minu4, _minu5);
      var urlJsonParser = Substitute.For<IGenericClassFactory<string, string, string>>();
      urlJsonParser.Create<TwitchStreamStatus.RootObject>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(_off, _on, _off, _on);
      var container = _getContainer(timeService, urlJsonParser);
      var streamStatusService = container.GetInstance<IStreamStatusService>();

      Assert.AreEqual(StreamStatus.Off, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.On, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.PossiblyOff, streamStatusService.Get());
      Assert.AreEqual(StreamStatus.On, streamStatusService.Get());

      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var onTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOnTime);
      Assert.AreEqual(_minu1, onTime);
      var offTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOffTime);
      Assert.AreEqual(_minu2, offTime);
      var currentState = unitOfWork.Query(u => u.StateIntegers.StreamStatus);
      Assert.AreEqual(StreamStatus.On, currentState);
    }

  }
}
