using System;
using System.Collections.Generic;
using Bot.Logic;
using Bot.Logic.Interfaces;
using Bot.Logic.SendablesFactoryVisitor;
using Bot.Logic.SnapshotFactoryVisitor;
using Bot.Models.Interfaces;
using Bot.Pipeline;
using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using SimpleInjector;

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var container = new Container();
      container.RegisterSingleton<INukeLogic, NukeLogic>();
      container.RegisterSingleton<IModCommandLogic, ModCommandLogic>();
      container.RegisterSingleton<IModCommandRegex, ModCommandRegex>();
      container.RegisterSingleton<IModCommandParser, ModCommandParser>();

      container.RegisterSingleton<IModCommandGenerator, ModCommandGenerator>();
      container.RegisterSingleton<ICommandGenerator, CommandGenerator>();
      container.RegisterSingleton<IBanGenerator, BanGenerator>();
      container.RegisterSingleton<ISendableGenerator, SendableGenerator>();

      container.RegisterSingleton<IReceivedToSnapshot, ReceivedToSnapshot>();
      container.RegisterSingleton<ISnapshotToSendable, SnapshotToSendable>();
      container.RegisterSingleton<ISender, ConsoleSender>();
      container.RegisterSingleton<IPipeline, Pipeline.Pipeline>();

      container.RegisterSingleton<ILogger, ConsoleLogger>();
      container.RegisterSingleton<ITimeService, TimeService>();
      container.RegisterSingleton<IReceivedFactory, ReceivedFactory>();
      container.RegisterSingleton<ISampleReceived, SampleReceived>();

      container.RegisterSingleton<IUserVisitor<IReceivedVisitor<SnapshotFactory>>, UserToSnapshotVisitor>();
      container.RegisterSingleton<ReceivedFromModeratorToSnapshotVisitor, ReceivedFromModeratorToSnapshotVisitor>();
      container.RegisterSingleton<ReceivedFromCivilianToSnapshotVisitor, ReceivedFromCivilianToSnapshotVisitor>();

      container.RegisterSingleton<IUserVisitor<IReceivedVisitor<SendablesFactory>>, UserToReceivedSendablesVisitor>();
      container.RegisterSingleton<CivilianReceivedToSendablesVisitor, CivilianReceivedToSendablesVisitor>();
      container.RegisterSingleton<ModeratorReceivedToSendablesVisitor, ModeratorReceivedToSendablesVisitor>();

      container.Verify();

      var data = container.GetInstance<ISampleReceived>();
      var pipeline = container.GetInstance<IPipeline>();
      pipeline.Run(data);

      Console.ReadLine();
    }
  }
}
