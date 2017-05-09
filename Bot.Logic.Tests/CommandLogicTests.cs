using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Tests;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class CommandLogicTests {

    [TestMethod]
    public void Blog_Returns_LatestEntry() {
      var time = new DateTime(2016, 10, 13, 20, 16, 17);
      var data = TestData.Blog;
      var downloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(data);
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(time);
      var testContainerManager = new TestContainerManager(c => {
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, c);
        c.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
        c.RegisterConditional<IGenericClassFactory<string, string, string>, UrlXmlParser>(Lifestyle.Singleton, _ => true);
        var downloaderRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), downloaderRegistration, _ => true);
      });
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"Current Streaming Set-up (October 2016)\" posted a few seconds ago https://blog.destiny.gg/current-streaming-set-up-october-2016/";

      var commandResponse = commandLogic.Blog();

      var actual = commandResponse.Transmission.Text;
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Streams_Returns_ThreeMostPopularStreams() {
      var data = TestData.OverRustle;
      var downloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(data);
      var testContainerManager = new TestContainerManager(c => {
        var downloaderRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), downloaderRegistration, _ => true);
      });
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = new List<string> {
        "137 overrustle.com/nomdeplume",
        "48 overrustle.com/a_real_human_bean",
        "9 overrustle.com/twitch/arteezy"
      };

      var commandResponse = commandLogic.Streams();

      Assert.IsTrue(commandResponse.Select(s => s.Transmission.Text).SequenceEqual(expected));
    }

  }
}
