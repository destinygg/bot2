using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Logic;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class CommandTests {
    [TestMethod]
    public void TwentyNineCivilianMessages_WithCommands_ExecutesOnlyThree() {
      var sender = new TestableSerializer();
      var containerManager = new TestContainerManager(container => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, container);
        container.RegisterConditional(typeof(ICommandHandler<IEnumerable<ISendable<ITransmittable>>>), senderRegistration, _ => true);
      }).InitializeAndIsolateRepository();
      var factory = containerManager.GetInstance<ReceivedFactory>();
      var pipeline = containerManager.GetInstance<IPipeline>();
      var data = new List<IReceived<IUser, ITransmittable>> {
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
        factory.PublicReceivedMessage("User11","!time"),
        factory.PublicReceivedMessage("User12","!time"),
        factory.PublicReceivedMessage("User13","!time"),
        factory.PublicReceivedMessage("User14","!time"),
        factory.PublicReceivedMessage("User15","!time"),
        factory.PublicReceivedMessage("User16","!time"),
        factory.PublicReceivedMessage("User17","!time"),
        factory.PublicReceivedMessage("User18","!time"),
        factory.PublicReceivedMessage("User19","!time"),
        factory.PublicReceivedMessage("User20","!time"),
        factory.PublicReceivedMessage("User21","!time"),
        factory.PublicReceivedMessage("User22","!time"),
        factory.PublicReceivedMessage("User23","!time"),
        factory.PublicReceivedMessage("User24","!time"),
        factory.PublicReceivedMessage("User25","!time"),
        factory.PublicReceivedMessage("User26","!time"),
        factory.PublicReceivedMessage("User27","!time"),
        factory.PublicReceivedMessage("User28","!time"),
        factory.PublicReceivedMessage("User29","!time"),
      };
      Task.Delay(2000).Wait();

      data.ForEach(x => {
        pipeline.Enqueue(x);
        Task.Delay(1000).Wait();
      });

      Task.Delay(1000).Wait();
      Assert.AreEqual(3, sender.Outbox.Count);
    }

  }
}
