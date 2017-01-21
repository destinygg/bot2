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
      container.Register<IBanGenerator, BanGenerator>();
      container.Register<ICommandGenerator, CommandGenerator>();
      container.Register<IModCommandGenerator, ModCommandGenerator>();
      container.Register<ISendableGenerator, SendableGenerator>();
      container.Register<ILogger, ConsoleLogger>();
      container.Register<IModCommandLogic, ModCommandLogic>();
      container.Register<IModCommandRegex, ModCommandRegex>(Lifestyle.Singleton);
      container.Register<IModCommandParser, ModCommandParser>();
      container.Register<ITimeService, TimeService>();
      container.Register<IReceivedFactory, ReceivedFactory>();
      container.Register<INukeLogic, NukeLogic>();
      container.Register<ISampleReceived, SampleReceived>();
      container.Register<IContextualizedToSendable, ContextualizedToSendable>();
      container.Register<IReceivedToContextualized, ReceivedToContextualized>();
      container.Register<ISender, ConsoleSender>();
      container.Register<IPipeline, Pipeline.Pipeline>();
      container.Verify();

      var data = container.GetInstance<ISampleReceived>();
      var pipeline = container.GetInstance<IPipeline>();
      pipeline.Run(data);

      Console.ReadLine();
    }
  }
}
