using System;
using Bot.Logic;
using Bot.Logic.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using log4net;

namespace Bot.Main.Moderate {
  public class DestinyGgListening : IExecutable {
    public void Execute() {
      var logger = LogManager.GetLogger(nameof(DestinyGgListening));
      logger.Info("Welcome to Bot!");
      logger.Info("Initializing...");

      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var pipeline = container.GetInstance<IPipeline>();
      var periodicTasks = container.GetInstance<PeriodicTasks>();
      var client = container.GetInstance<IClient>();
      var twitterManager = container.GetInstance<ITwitterManager>();
      pipeline.SetSender(client.Send);
      client.SetReceive(pipeline.Enqueue);

      logger.Info("Initialization complete.");
      logger.Info("Running...\r\n\r\n");

      client.Connect();
      periodicTasks.Run();
      twitterManager.MonitorNewTweets(pipeline.Enqueue);

      var r = container.GetInstance<ReceivedFactory>();
      while (true) {
        var line = Console.ReadLine();
        var message = r.ModPublicReceivedMessage(line);
        pipeline.Enqueue(message);
      }
    }

  }
}
