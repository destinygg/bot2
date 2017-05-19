using System;
using Bot.Logic;
using Bot.Logic.Interfaces;
using Bot.Pipeline;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using log4net;

namespace Bot.Main.Moderate {
  public class DestinyGgListening : IExecutable {
    private readonly bool _canSend;
    private readonly bool _runTwitter;

    public DestinyGgListening(bool canSend, bool runTwitter) {
      _canSend = canSend;
      _runTwitter = runTwitter;
    }


    public void Execute() {
      var logger = LogManager.GetLogger(nameof(DestinyGgListening));
      logger.Info("Welcome to Bot!");
      logger.Info("Initializing...");

      var container = new TestContainerManager(c => {
        if (_canSend) c.RegisterConditional(typeof(IClient), typeof(DestinyGgSendingClient), pc => !pc.Handled);
      }, s => {
        s.SqlitePath = "Bot.sqlite";
      }).Container;

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
      if (_runTwitter) twitterManager.MonitorNewTweets(pipeline.Enqueue);

      var r = container.GetInstance<ReceivedFactory>();
      while (true) {
        var line = Console.ReadLine();
        var message = r.ModPublicReceivedMessage(line);
        pipeline.Enqueue(message);
      }
    }

  }
}
