using System;
using Bot.Logic;
using Bot.Logic.Contracts;
using Bot.Pipeline;
using Bot.Pipeline.Contracts;
using SimpleInjector;

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var container = new Container();
      container.Register<IReceivedProducer, SampleReceivedProducer>();
      container.Register<IContextualizedProducer, ContextualizedProducer>();
      container.Register<ISendableProducer, SendableProducer>();
      container.Register<IScanForBans, ScanForBans>();
      container.Register<IScanForCommands, ScanForCommands>();
      container.Register<IScanForModCommands, ScanForModCommands>();
      container.Register<IContextualizedProcessor, ContextualizedProcessor>();
      container.Register<ILogger, ConsoleLogger>();
      container.Verify();

      var sender = container.GetInstance<ConsoleSender>();
      Console.ReadLine();
    }
  }
}
