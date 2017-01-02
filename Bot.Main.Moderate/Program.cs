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
      container.Register<IBanGenerator, BanGenerator>();
      container.Register<ICommandGenerator, CommandGenerator>();
      container.Register<IModCommandGenerator, ModCommandGenerator>();
      container.Register<ISendableGenerator, SendableGenerator>();
      container.Register<ILogger, ConsoleLogger>();
      container.Register<ISender, ConsoleSender>();
      container.Register<IModCommands, ModCommands>();
      container.Verify();

      var sender = container.GetInstance<ISender>();
      var sendableProducer = container.GetInstance<ISendableProducer>();
      sender.Send(sendableProducer);
      Console.ReadLine();
    }
  }
}
