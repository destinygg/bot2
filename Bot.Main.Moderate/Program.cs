using System;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var logger = LogManager.GetLogger("Main");
      logger.Info("Welcome to Bot!");
      logger.Info("Initializing...");

      var containerManager = new ContainerManager();
      var data = containerManager.SampleReceived;
      var pipeline = containerManager.Pipeline;

      logger.Info("Initialization complete.");
      logger.Info("Running...\r\n\r\n");

      pipeline.Run(data);

      Console.ReadLine();
    }
  }
}
