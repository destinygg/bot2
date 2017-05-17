using System;
using System.Linq;
using Bot.Models;
using Bot.Models.Json;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Tools.Tests {
  [TestClass]
  public class UrlJsonParserTests {

    private TestContainerManager CreateTestContainerManager(string data) {
      var downloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(data);
      return new TestContainerManager(c => {
        c.RegisterConditional<IGenericClassFactory<string, string, string>, UrlJsonParser>(Lifestyle.Singleton, _ => true);
        var downloaderRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), downloaderRegistration, _ => true);
      });
    }

    [TestMethod]
    public void UrlJsonParser_TwitchLiveStatus_ViewersWorks() {
      var data = TestData.TwitchOnline;
      var testContainerManager = CreateTestContainerManager(data);
      var urlJsonParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();

      var twitchLiveStatus = urlJsonParser.Create<TwitchStreamStatus.RootObject>("", "", "");

      Assert.AreEqual(2557, twitchLiveStatus.stream.viewers);
    }

    [TestMethod]
    public void UrlJsonParser_TwitchLiveStatus_ParsedCreatedAtWorks() {
      var data = TestData.TwitchOnline;
      var testContainerManager = CreateTestContainerManager(data);
      var urlJsonParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();

      var parsedCreatedAt = urlJsonParser.Create<TwitchStreamStatus.RootObject>("", "", "").stream.Parsed_created_at;

      Assert.AreEqual(new DateTime(2017, 04, 24, 21, 23, 16, DateTimeKind.Utc), parsedCreatedAt);
    }

    [TestMethod]
    public void UrlJsonParser_LastFmNotPlaying_ParsedUtsWorks() {
      var data = TestData.LastFmNotPlaying;
      var testContainerManager = CreateTestContainerManager(data);
      var urlJsonParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();
      var expected = new DateTime(2017, 5, 14, 7, 48, 54);

      var rootObject = urlJsonParser.Create<LastFm.RootObject>("", "", "");

      var first = rootObject.recenttracks.track.First();
      Assert.AreEqual(expected, first.date.Parsed_uts);
    }

    [TestMethod]
    public void UrlJsonParser_LastFmNotPlaying_IsNotPlaying() {
      var data = TestData.LastFmNotPlaying;
      var testContainerManager = CreateTestContainerManager(data);
      var urlJsonParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();

      var rootObject = urlJsonParser.Create<LastFm.RootObject>("", "", "");

      var first = rootObject.recenttracks.track.First();
      Assert.IsFalse(first.NowPlaying);
    }

    [TestMethod]
    public void UrlJsonParser_LastFmPlaying_IsPlaying() {
      var data = TestData.LastFmPlaying;
      var testContainerManager = CreateTestContainerManager(data);
      var urlJsonParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();

      var rootObject = urlJsonParser.Create<LastFm.RootObject>("", "", "");

      var first = rootObject.recenttracks.track.First();
      Assert.IsTrue(first.NowPlaying);
    }

    [TestMethod]
    public void StreamState_TwitchOnline_CanBeCreated() {
      var data = TestData.TwitchOnline;
      var testContainerManager = CreateTestContainerManager(data);
      var urlJsonParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();
      var twitchLiveStatus = urlJsonParser.Create<TwitchStreamStatus.RootObject>("", "", "");

      new StreamState(StreamStatus.On, twitchLiveStatus);
    }

    [TestMethod]
    public void StreamState_TwitchOffline_CanBeCreated() {
      var data = TestData.TwitchOffline;
      var testContainerManager = CreateTestContainerManager(data);
      var urlJsonParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();
      var twitchLiveStatus = urlJsonParser.Create<TwitchStreamStatus.RootObject>("", "", "");

      new StreamState(StreamStatus.Off, twitchLiveStatus);
    }

  }
}
