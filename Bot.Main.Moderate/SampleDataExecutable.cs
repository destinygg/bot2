using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Logic;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using log4net;

namespace Bot.Main.Moderate {
  public class SampleDataExecutable : IExecutable {
    public void Execute() {
      var logger = LogManager.GetLogger(nameof(SampleDataExecutable));
      logger.Info("Welcome to Bot!");
      logger.Info("Initializing...");

      var container = new TestContainerManager(
        configureSettings: s => s.ClientType = nameof(SampleDataExecutable)
        ).InitializeAndIsolateRepository();
      var factory = container.GetInstance<ReceivedFactory>();
      var pipelineManager = container.GetInstance<IPipelineManager>();
      var periodicTaskRunner = container.GetInstance<PeriodicTaskRunner>();

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
        pipelineManager.Enqueue(x);
      });
      periodicTaskRunner.Run();
    }

  }
}
