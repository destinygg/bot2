using System;
using Bot.Logic;
using Bot.Logic.Interfaces;
using Bot.Pipeline;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using log4net;

namespace Bot.Main.Moderate {
  public class DestinyGgExecutable : IExecutable {
    private readonly bool _canSend;
    private readonly bool _runTwitter;

    public DestinyGgExecutable(bool canSend, bool runTwitter) {
      _canSend = canSend;
      _runTwitter = runTwitter;
    }


    public void Execute() {
      var logger = LogManager.GetLogger(nameof(DestinyGgExecutable));
      logger.Info("Welcome to Bot!");
      logger.Info("Initializing...");

      var container = new TestContainerManager(c => {
        if (_canSend) c.RegisterConditional(typeof(IClient), typeof(DestinyGgSendingClient), pc => !pc.Handled);
      }, s => {
        s.SqlitePath = "Bot.sqlite";
        s.ClientType = nameof(DestinyGgExecutable);
      }).Container;

      var pipelineManager = container.GetInstance<IPipelineManager>();
      var periodicTasks = container.GetInstance<PeriodicTasks>();
      var client = container.GetInstance<IClient>();
      var twitterManager = container.GetInstance<ITwitterManager>();
      pipelineManager.SetSender(client.Send);
      client.SetReceive(pipelineManager.Enqueue);

      logger.Info("Initialization complete.");
      logger.Info("Running...\r\n\r\n");

      client.Connect();
      periodicTasks.Run();
      if (_runTwitter) twitterManager.MonitorNewTweets(pipelineManager.Enqueue);

      var r = container.GetInstance<ReceivedFactory>();
      while (true) {
        var line = Console.ReadLine();
        var message = r.ModPublicReceivedMessage(line);
        pipelineManager.Enqueue(message);
      }
    }

  }
}
