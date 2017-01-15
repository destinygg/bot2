using System;
using Bot.Logic;
using Bot.Logic.Contracts;
using Bot.Pipeline;
using Bot.Pipeline.Contracts;
using Bot.Tools;
using Bot.Tools.Contracts;
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
      container.Register<IModCommandLogic, ModCommandLogic>();
      container.Register<IModCommandRegex, ModCommandRegex>(Lifestyle.Singleton);
      container.Register<IModCommandParser, ModCommandParser>();
      container.Register<ITimeService, TimeService>();
      container.Verify();

      var sender = container.GetInstance<ISender>();
      var sendableProducer = container.GetInstance<ISendableProducer>();
      sender.Send(sendableProducer);
      Console.ReadLine();
    }
  }
}
