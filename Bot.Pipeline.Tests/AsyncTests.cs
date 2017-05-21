using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logic;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using Bot.Tools.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class AsyncTests {

    [TestMethod]
    public async Task Async_Long_IsOrdered() {
      var testableLogger = new TestableLogger();
      var containerManager = new TestContainerManager(
        container => {
          var settingsServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => testableLogger, container);
          container.RegisterConditional(typeof(ILogger), settingsServiceRegistration, pc => !pc.Handled);
        });
      var factory = containerManager.Container.GetInstance<ReceivedFactory>();
      var pipelineManager = containerManager.Container.GetInstance<IPipelineManager>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.ModPublicReceivedMessage("!long"),
        factory.ModPublicReceivedMessage("!long"),
        factory.ModPublicReceivedMessage("!long"),
      };

      data.ForEach(x => pipelineManager.Enqueue(x));

      await Task.Delay(15000);
      var results = testableLogger.Outbox.Where(s => s == "#1" || s == "#2" || s == "#3").ToList();
      Assert.AreEqual(9, results.Count);
      var alphabetized = results.OrderBy(q => q).ToList();
      Assert.IsTrue(results.SequenceEqual(alphabetized));
    }

  }
}
