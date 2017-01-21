using System;
using Bot.Logic;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Pipeline;
using Bot.Pipeline.Contracts;
using Bot.Tools;
using Bot.Tools.Contracts;
using SimpleInjector;

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var container = new Container();
      container.Register<IReceivedProducer, SampleReceivedProducer>(Lifestyle.Singleton);
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
      container.Register<IReceivedFactory, ReceivedFactory>();
      container.Register<INukeLogic, NukeLogic>();
      container.Register<ISampleReceived, SampleReceived>();
      container.Verify();

      var sampleRecieved = container.GetInstance<IReceivedProducer>();
      var data = container.GetInstance<ISampleReceived>();
      sampleRecieved.Run(data);

      var sender = container.GetInstance<ISender>();
      sender.Run();
      
      Console.ReadLine();
    }
  }
}
