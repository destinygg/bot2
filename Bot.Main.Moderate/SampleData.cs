using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Logic;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using log4net;

namespace Bot.Main.Moderate {
  public class SampleData : IExecutable {
    public void Execute() {
      var logger = LogManager.GetLogger(nameof(SampleData));
      logger.Info("Welcome to Bot!");
      logger.Info("Initializing...");

      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var factory = container.GetInstance<ReceivedFactory>();
      var pipeline = container.GetInstance<IPipeline>();
      var periodicTasks = container.GetInstance<PeriodicTasks>();

      logger.Info("Initialization complete.");
      logger.Info("Running...\r\n\r\n");

      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.ModPublicReceivedMessage("!long"),
        factory.PublicReceivedMessage("hi"),
        factory.PublicReceivedMessage("banplox"),
        factory.PublicReceivedMessage("!time"),
        factory.ModPublicReceivedMessage("!sing"),
        factory.ModPublicReceivedMessage("!long"),
      };

      data.ForEach(x => {
        Task.Delay(100).Wait();
        pipeline.Enqueue(x);
      });
      periodicTasks.Run();
    }

  }
}
