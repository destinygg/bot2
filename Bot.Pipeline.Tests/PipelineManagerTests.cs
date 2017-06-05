using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Main.Moderate;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class PipelineManagerTests {

    private void Run(List<IReceived<IUser, ITransmittable>> data, IPipelineManager pipelineManager, TestableSerializer sender) {
      Task.Delay(100).Wait();

      data.ForEach(x => {
        Task.Delay(100).Wait();
        pipelineManager.Enqueue(x);
      });

      Task.Delay(100).Wait();
      foreach (var sendable in sender.Outbox) {
        Console.WriteLine(sendable);
      }
    }

    [TestMethod]
    public void PipelineManager_ReceivedError_SendsIt() {
      var expectedErrorMessage = "A random error";
      var sender = new TestableSerializer();
      var containerManager = new TestContainerManager(container => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, container);
        container.RegisterConditional(typeof(IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>), senderRegistration, _ => true);
      }).Container;
      var pipelineManager = containerManager.GetInstance<IPipelineManager>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        new ReceivedError(expectedErrorMessage,DateTime.UtcNow),
      };

      Run(data, pipelineManager, sender);

      Assert.AreEqual(expectedErrorMessage, sender.Outbox.Cast<SendablePublicMessage>().Single().Text);
    }

  }
}
