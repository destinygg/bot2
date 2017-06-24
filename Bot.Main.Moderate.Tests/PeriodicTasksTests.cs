using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
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
      var errorableDownloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      errorableDownloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(TestData.YoutubeFeed);
      var sender = new TestableSerializer();
      var container = new TestContainerManager(c => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, c);
        c.RegisterConditional(typeof(IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>), senderRegistration, _ => true);
        var errorableDownloadFactoryRegistration = Lifestyle.Singleton.CreateRegistration(() => errorableDownloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), errorableDownloadFactoryRegistration, _ => true);
      }, settings => settings.PeriodicMessageInterval = TimeSpan.FromMilliseconds(100))
      .InitializeAndIsolateRepository();
      var periodicTaskRunner = container.GetInstance<PeriodicTaskRunner>();

      periodicTaskRunner.Run();

      Task.Delay(1000).Wait();
      var cache = "";
      foreach (var sendable in sender.Outbox.Cast<SendablePublicMessage>()) {
        if (sendable.Text == cache) Assert.Fail();
        cache = sendable.Text;
      }
    }

    [TestMethod]
    public void PeriodicTasks_YoutubeDown_DecoratorReportsErrors() {
      var errorableDownloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      errorableDownloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns("");
      var sender = new TestableSerializer();
      var testableLogger = new TestableLogger();
      var container = new TestContainerManager(c => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, c);
        c.RegisterConditional(typeof(IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>), senderRegistration, _ => true);
        var errorableDownloadFactoryRegistration = Lifestyle.Singleton.CreateRegistration(() => errorableDownloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), errorableDownloadFactoryRegistration, _ => true);
        var loggerRegistration = Lifestyle.Singleton.CreateRegistration(() => testableLogger, c);
        c.RegisterConditional(typeof(ILogger), loggerRegistration, pc => !pc.Handled);
      }, settings => settings.PeriodicMessageInterval = TimeSpan.FromMilliseconds(100))
      .InitializeAndIsolateRepository();
      var periodicTaskRunner = container.GetInstance<PeriodicTaskRunner>();

      periodicTaskRunner.Run();

      Task.Delay(900).Wait();
      Console.WriteLine(ObjectDumper.Dump(testableLogger.Outbox));
      Assert.IsTrue(testableLogger.Outbox.Any(x => x.Contains("Error occured in GenericClassFactoryTryCatchDecorator")));
      Assert.IsTrue(testableLogger.Outbox.Any(x => x.Contains("input1 is \"https://www.youtube.com/feeds/videos.xml?user=destiny\"")));
    }

  }
}
