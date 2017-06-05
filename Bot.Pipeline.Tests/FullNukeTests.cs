using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logic;
using Bot.Main.Moderate;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class FullNukeTests {

    [TestMethod]
    public void NukePunishes_UsersAfter_Always() {
      var sender = new TestableSerializer();
      var containerManager = new TestContainerManager(container => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, container);
        container.RegisterConditional(typeof(IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>), senderRegistration, _ => true);
      }).InitializeAndIsolateRepository();
      var factory = containerManager.GetInstance<ReceivedFactory>();
      var pipelineManager = containerManager.GetInstance<IPipelineManager>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.ModPublicReceivedMessage("!nuke banplox"),
        factory.PublicReceivedMessage("User", "banplox"),
      };
      Task.Delay(1000).Wait();

      data.ForEach(x => {
        Task.Delay(1000).Wait();
        pipelineManager.Enqueue(x);
      });

      Task.Delay(1000).Wait();
      Assert.IsTrue(sender.Outbox.Cast<SendableMute>().First().Target.Nick == "User");
    }

    [TestMethod]
    public void NukesExpire_Always_AfterSomeTime() {
      var sender = new TestableSerializer();
      var containerManager = new TestContainerManager(container => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, container);
        container.RegisterConditional(typeof(IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>), senderRegistration, _ => true);
      }, settings => {
        settings.NukeMaximumLinger = TimeSpan.FromSeconds(5);
      }).InitializeAndIsolateRepository();
      var factory = containerManager.GetInstance<ReceivedFactory>();
      var pipelineManager = containerManager.GetInstance<IPipelineManager>();
      var timeService = containerManager.GetInstance<ITimeService>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.ModPublicReceivedMessage("!nuke !time"),
        factory.PublicReceivedMessage("User01","!time"),
        factory.PublicReceivedMessage("User02","!time"),
        factory.PublicReceivedMessage("User03","!time"),
        factory.PublicReceivedMessage("User04","!time"),
        factory.PublicReceivedMessage("User05","!time"),
        factory.PublicReceivedMessage("User06","!time"),
        factory.PublicReceivedMessage("User07","!time"),
        factory.PublicReceivedMessage("User08","!time"),
        factory.PublicReceivedMessage("User09","!time"),
        factory.PublicReceivedMessage("User10","!time"),
      };
      Task.Delay(1000).Wait();

      data.ForEach(x => {
        Task.Delay(1000).Wait();
        pipelineManager.Enqueue(x);
      });

      Task.Delay(1000).Wait();
      Assert.IsTrue(sender.Outbox.OfType<SendableMute>().Count() >= 4);
      Assert.IsTrue(sender.Outbox.OfType<SendableMute>().Count() <= 8);
      Assert.IsTrue(sender.Outbox.OfType<SendablePublicMessage>().Single().Text.Contains(timeService.DestinyNow.ToShortTimeString()));
    }

    [TestMethod]
    public void Aegis_Always_PreventsMoreMutes() {
      var sender = new TestableSerializer();
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(TestHelper.RandomDateTime());
      var containerManager = new TestContainerManager(container => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, container);
        container.RegisterConditional(typeof(IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>), senderRegistration, _ => true);
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, container);
        container.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
      }).InitializeAndIsolateRepository();
      var factory = containerManager.GetInstance<ReceivedFactory>();
      var pipelineManager = containerManager.GetInstance<IPipelineManager>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.ModPublicReceivedMessage("!nuke !time"),
        factory.PublicReceivedMessage("User01","!time"),
        factory.PublicReceivedMessage("User02","!time"),
        factory.PublicReceivedMessage("User03","!time"),
        factory.PublicReceivedMessage("User04","!time"),
        factory.PublicReceivedMessage("User05","!time"),
        factory.ModPublicReceivedMessage("!aegis"),
        factory.PublicReceivedMessage("User06","!time"),
        factory.PublicReceivedMessage("User07","!time"),
        factory.PublicReceivedMessage("User08","!time"),
        factory.PublicReceivedMessage("User09","!time"),
        factory.PublicReceivedMessage("User10","!time"),
      };
      Task.Delay(1000).Wait();

      data.ForEach(x => {
        Task.Delay(1000).Wait();
        pipelineManager.Enqueue(x);
      });

      Task.Delay(1000).Wait();
      Assert.IsTrue(sender.Outbox.OfType<SendableMute>().Count() >= 4);
      Assert.IsTrue(sender.Outbox.OfType<SendableMute>().Count() <= 8);
      Assert.IsTrue(sender.Outbox.OfType<SendablePardon>().Count() >= 4);
      Assert.IsTrue(sender.Outbox.OfType<SendablePardon>().Count() <= 8);
      Assert.IsTrue(sender.Outbox.OfType<SendablePublicMessage>().Single().Text.Contains(timeService.DestinyNow.ToShortTimeString()));
    }

  }
}
