using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class CommandLogicTests {

    private TestContainerManager _createTestContainerManager(string data, DateTime? possibleTime = null) {
      var time = possibleTime ?? DateTime.UtcNow;
      var errorableDownloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      errorableDownloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(data);
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(time);
      return new TestContainerManager(c => {
        var errorableDownloadFactoryRegistration = Lifestyle.Singleton.CreateRegistration(() => errorableDownloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), errorableDownloadFactoryRegistration, _ => true);
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, c);
        c.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
      });
    }

    [TestMethod]
    public void Blog_Returns_LatestEntry() {
      var time = new DateTime(2016, 10, 13, 20, 16, 17);
      var data = TestData.Blog;
      var testContainerManager = _createTestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"Current Streaming Set-up (October 2016)\" posted a few seconds ago https://blog.destiny.gg/current-streaming-set-up-october-2016/";

      var commandResponse = commandLogic.Blog();

      var actual = commandResponse.Transmission.Text;
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Streams_Returns_ThreeMostPopularStreams() {
      var data = TestData.OverRustle;
      var testContainerManager = _createTestContainerManager(data);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = new List<string> {
        "137 overrustle.com/nomdeplume",
        "48 overrustle.com/a_real_human_bean",
        "9 overrustle.com/twitch/arteezy"
      };

      var commandResponse = commandLogic.Streams();

      Assert.IsTrue(commandResponse.Select(s => s.Transmission.Text).SequenceEqual(expected));
    }

    [TestMethod]
    public void Song_NotPlaying_ReturnsMostRecentlyPlayedSong() {
      var data = TestData.LastFmNotPlaying;
      var time = new DateTime(2017, 5, 15, 4, 1, 0);
      var expected = "No song played/scrobbled. Played 20h 12m ago: beauty - Nymano";
      var testContainerManager = _createTestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();

      var commandResponse = commandLogic.Song();

      Assert.AreEqual(expected, commandResponse.OfType<SendablePublicMessage>().Single().Text);
    }

    [TestMethod]
    public void Song_NowPlaying_ReturnsCurrentlyPlayingSong() {
      var data = TestData.LastFmPlaying;
      var expected = "Harambe - Dumbfoundead last.fm/user/stevenbonnellii";
      var testContainerManager = _createTestContainerManager(data);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();

      var commandResponse = commandLogic.Song();

      Assert.AreEqual(expected, commandResponse.OfType<SendablePublicMessage>().Single().Text);
    }

    [TestMethod]
    public void PreviousSong_NotPlaying_ReturnsMostRecentlyPlayedSong() {
      var data = TestData.LastFmNotPlaying;
      var time = new DateTime(2017, 5, 15, 4, 1, 0);
      var expected = "truly happy - Nymano played 20h 13m ago before beauty - Nymano";
      var testContainerManager = _createTestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();

      var commandResponse = commandLogic.PreviousSong();

      Assert.AreEqual(expected, commandResponse.OfType<SendablePublicMessage>().Single().Text);
    }

    [TestMethod]
    public void PreviousSong_NowPlaying_ReturnsCurrentlyPlayingSong() {
      var data = TestData.LastFmPlaying;
      var time = new DateTime(2017, 5, 15, 4, 1, 0);
      var expected = "Viva la Vida - Coldplay played 1h 23m ago before Harambe - Dumbfoundead";
      var testContainerManager = _createTestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();

      var commandResponse = commandLogic.PreviousSong();

      Assert.AreEqual(expected, commandResponse.OfType<SendablePublicMessage>().Single().Text);
    }

  }
}
