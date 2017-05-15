using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logic;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class CustomCommandTests {

    private Container CreateContainer(TestableSerializer sender) {
      var containerManager = new TestContainerManager(container => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, container);
        container.RegisterConditional(typeof(IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>),
          senderRegistration, _ => true);
      }, settings => {
        settings.CivilianCommandInterval = TimeSpan.FromSeconds(0);
      }).InitializeAndIsolateRepository();
      return containerManager;
    }

    private void Run(List<IReceived<IUser, ITransmittable>> data, IPipeline pipeline, TestableSerializer sender) {
      Task.Delay(1000).Wait();

      data.ForEach(x => {
        Task.Delay(1000).Wait();
        pipeline.Enqueue(x);
      });

      Task.Delay(1000).Wait();
      foreach (var sendable in sender.Outbox) {
        Console.WriteLine(sendable);
      }
    }

    [TestMethod]
    public void AddingCommand_Afterwards_GetsResponse() {
      var sender = new TestableSerializer();
      var containerManager = CreateContainer(sender);
      var factory = containerManager.GetInstance<ReceivedFactory>();
      var pipeline = containerManager.GetInstance<IPipeline>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.ModPublicReceivedMessage("!addcommand !hi greetings"),
        factory.PublicReceivedMessage("!hi"),
      };

      Run(data, pipeline, sender);

      Assert.IsTrue(sender.Outbox.Cast<SendablePublicMessage>().First().Text == "!hi added");
      Assert.IsTrue(sender.Outbox.Cast<SendablePublicMessage>().Skip(1).First().Text == "greetings");
    }

    [TestMethod]
    public void AddingCommand_WithSpace_GetsResponse() {
      var sender = new TestableSerializer();
      var containerManager = CreateContainer(sender);
      var factory = containerManager.GetInstance<ReceivedFactory>();
      var pipeline = containerManager.GetInstance<IPipeline>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.ModPublicReceivedMessage("!addcommand !hi greetings"),
        factory.PublicReceivedMessage("! hi"),
      };

      Run(data, pipeline, sender);

      Assert.IsTrue(sender.Outbox.Cast<SendablePublicMessage>().First().Text == "!hi added");
      Assert.IsTrue(sender.Outbox.Cast<SendablePublicMessage>().Skip(1).First().Text == "greetings");
    }

    [TestMethod]
    public void UpdatingCommand_Afterwards_GetsDifferentResponse() {
      var sender = new TestableSerializer();
      var containerManager = CreateContainer(sender);
      var factory = containerManager.GetInstance<ReceivedFactory>();
      var pipeline = containerManager.GetInstance<IPipeline>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.ModPublicReceivedMessage("!addcommand !hi greetings"),
        factory.PublicReceivedMessage("!hi"),
        factory.ModPublicReceivedMessage("!addcommand !hi bonjour"),
        factory.PublicReceivedMessage("!hi"),
      };

      Run(data, pipeline, sender);

      Assert.AreEqual("!hi added", sender.Outbox.Cast<SendablePublicMessage>().First().Text);
      Assert.AreEqual("greetings", sender.Outbox.Cast<SendablePublicMessage>().Skip(1).First().Text);
      Assert.AreEqual("!hi updated", sender.Outbox.Cast<SendablePublicMessage>().Skip(2).First().Text);
      Assert.AreEqual("bonjour", sender.Outbox.Cast<SendablePublicMessage>().Skip(3).First().Text);
    }

    [TestMethod]
    public void DeletingCommand_Afterwards_GetsNoResponse() {
      var sender = new TestableSerializer();
      var containerManager = CreateContainer(sender);
      var factory = containerManager.GetInstance<ReceivedFactory>();
      var pipeline = containerManager.GetInstance<IPipeline>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.PublicReceivedMessage("!rules"),
        factory.ModPublicReceivedMessage("!delcommand !rules"),
        factory.PublicReceivedMessage("!rules"),
        factory.PublicReceivedMessage("!rules"),
        factory.PublicReceivedMessage("!rules"),
      };

      Run(data, pipeline, sender);

      Assert.AreEqual("github.com/destinygg/bot2", sender.Outbox.Cast<SendablePublicMessage>().First().Text);
      Assert.AreEqual("!rules removed", sender.Outbox.Cast<SendablePublicMessage>().Skip(1).First().Text);
      Assert.AreEqual(2, sender.Outbox.Count);
    }

  }
}
