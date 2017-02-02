﻿using System;
using Bot.Logic;
using Bot.Logic.Interfaces;
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

      container.RegisterSingleton<IUserVisitor<IReceivedVisitor>, UserToReceivedVisitor>();
      container.RegisterSingleton<CivilianReceivedVisitor, CivilianReceivedVisitor>();
      container.RegisterSingleton<ModeratorReceivedVisitor, ModeratorReceivedVisitor>();

      container.Verify();

      var data = container.GetInstance<ISampleReceived>();
      var pipeline = container.GetInstance<IPipeline>();
      pipeline.Run(data);

      Console.ReadLine();
    }
  }
}
