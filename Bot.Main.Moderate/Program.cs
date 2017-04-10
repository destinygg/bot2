using System;
using System.Collections.Generic;
using Bot.Logic;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var logger = LogManager.GetLogger("Main");
      logger.Info("Welcome to Bot!");
      logger.Info("Initializing...");

      var containerManager = new TestContainerManager();
      var factory = containerManager.Container.GetInstance<ReceivedFactory>();
      var pipeline = containerManager.Container.GetInstance<IPipeline>();

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

      pipeline.Run(data);

      Console.ReadLine();
    }
  }
}
