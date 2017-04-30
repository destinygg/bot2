using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Pipeline.Tests;
using Bot.Tests;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Main.Moderate.Tests {
  [TestClass]
  public class PeriodicTasksTests {

    [TestMethod]
    public void PeriodicTasks_Run_YieldsAlternatingMessages() {
      var downloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(TestHelper.YoutubeFeed);
      var sender = new TestableSender();
      var container = new TestContainerManager(c => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, c);
        c.RegisterConditional(typeof(ICommandHandler<IEnumerable<ISendable<ITransmittable>>>), senderRegistration, _ => true);
        var downloaderRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), downloaderRegistration, _ => true);
      }, settings => settings.PeriodicTaskInterval = TimeSpan.FromMilliseconds(100))
      .InitializeAndIsolateRepository();
      var tasks = container.GetInstance<PeriodicTasks>();

      tasks.Run();

      Task.Delay(1000).Wait();
      var cache = "";
      foreach (var sendable in sender.Outbox.Cast<SendablePublicMessage>()) {
        if (sendable.Text == cache) Assert.Fail();
        cache = sendable.Text;
      }
    }

    [TestMethod]
    public void PeriodicTasks_YoutubeDown_DecoratorReportsErrors() {
      var downloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns("");
      var sender = new TestableSender();
      var testableLogger = new TestableLogger();
      var container = new TestContainerManager(c => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, c);
        c.RegisterConditional(typeof(ICommandHandler<IEnumerable<ISendable<ITransmittable>>>), senderRegistration, _ => true);
        var downloaderRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), downloaderRegistration, _ => true);
        var loggerRegistration = Lifestyle.Singleton.CreateRegistration(() => testableLogger, c);
        c.RegisterConditional(typeof(ILogger), loggerRegistration, pc => !pc.Handled);
      }, settings => settings.PeriodicTaskInterval = TimeSpan.FromMilliseconds(100))
      .InitializeAndIsolateRepository();
      var tasks = container.GetInstance<PeriodicTasks>();

      tasks.Run();

      Task.Delay(900).Wait();
      Console.WriteLine(ObjectDumper.Dump(testableLogger.Outbox));
      Assert.IsTrue(testableLogger.Outbox.Any(x => x.Contains("Error occured in GenericClassFactoryTryCatchDecorator")));
      Assert.IsTrue(testableLogger.Outbox.Any(x => x.Contains("input1 is \"https://www.youtube.com/feeds/videos.xml?user=destiny\"")));
    }

  }
}
