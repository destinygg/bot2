using System;
using System.Collections.Generic;
using Bot.Logic;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Pipeline;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using log4net;
using SimpleInjector;

namespace Bot.Main.Moderate {
  public class ContinuousExecutable : IExecutable {
    private readonly bool _canSend;
    private readonly bool _runTwitter;
    private readonly bool _isDestinyGg;

    public ContinuousExecutable(bool canSend, bool runTwitter, bool isDestinyGg) {
      _canSend = canSend;
      _runTwitter = runTwitter;
      _isDestinyGg = isDestinyGg;
    }

    public void Execute() {
      var logger = LogManager.GetLogger(nameof(ContinuousExecutable));
      logger.Info("Welcome to Bot!");
      logger.Info("Initializing...");

      var container = new TestContainerManager(c => {
        if (_isDestinyGg) {
          if (_canSend) {
            c.RegisterConditional(typeof(IClient), typeof(DestinyGgSendingClient), Lifestyle.Singleton, pc => !pc.Handled);
          }
        } else {
          c.RegisterConditional(typeof(IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>), typeof(TwitchSerializer), Lifestyle.Singleton, pc => !pc.Handled);
          if (_canSend) {
            c.RegisterConditional(typeof(IClient), typeof(TwitchSendingClient), Lifestyle.Singleton, pc => !pc.Handled);
          } else {
            c.RegisterConditional(typeof(IClient), typeof(TwitchLoggingClient), Lifestyle.Singleton, pc => !pc.Handled);
          }
        }
      }, s => {
        s.SqlitePath = "Bot.sqlite";
        s.ClientType = _isDestinyGg ? "DestinyGg" : "Twitch";
      }).Container;

      var pipelineManager = container.GetInstance<IPipelineManager>();
      var periodicTasks = container.GetInstance<PeriodicTasks>();
      var client = container.GetInstance<IClient>();
      var twitterManager = container.GetInstance<ITwitterManager>();

      logger.Info("Initialization complete.");
      logger.Info("Running...\r\n\r\n");

      client.Connect();
      periodicTasks.Run();
      if (_runTwitter) twitterManager.MonitorNewTweets(pipelineManager.Enqueue);

      var r = container.GetInstance<ReceivedFactory>();
      while (true) {
        var line = Console.ReadLine();
        var message = line.StartsWith("~")
          ? (IReceived<User, ITransmittable>) r.PublicReceivedMessage(line.Substring(1))
          : (IReceived<User, ITransmittable>) r.ModPublicReceivedMessage(line);

        pipelineManager.Enqueue(message);
      }
    }

  }
}
