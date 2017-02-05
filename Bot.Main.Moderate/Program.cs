using System;
using System.Collections.Generic;
using Bot.Logic;
using Bot.Logic.Interfaces;
using Bot.Logic.ReceivedVisitor;
using Bot.Logic.SnapshotVisitor;
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

      container.RegisterSingleton<IFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>, SnapshotFactory>();
      container.RegisterSingleton<IFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>, SendablesFactory>();
      container.RegisterSingleton<ISender, ConsoleSender>();
      container.RegisterSingleton<IPipeline, Pipeline.Pipeline>();

      container.RegisterSingleton<ILogger, Logger>();
      container.RegisterSingleton<ILogFormatter, LogFormatter>();
      container.RegisterSingleton<ILogPersister, ConsolePersister>();

      container.RegisterSingleton<ITimeService, TimeService>();
      container.RegisterSingleton<IReceivedFactory, ReceivedFactory>();
      container.RegisterSingleton<ISampleReceived, SampleReceived>();

      container.RegisterSingleton<IUserVisitor<IReceivedVisitor<DelegatedSnapshotFactory>>, Logic.ReceivedVisitor.UserVisitor>();
      container.RegisterSingleton<ModeratorReceivedVisitor, ModeratorReceivedVisitor>();
      container.RegisterSingleton<CivilianReceivedVisitor, CivilianReceivedVisitor>();

      container.RegisterSingleton<IUserVisitor<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>>, Logic.SnapshotVisitor.UserVisitor>();
      container.RegisterSingleton<CivilianSnapshotVisitor, CivilianSnapshotVisitor>();
      container.RegisterSingleton<ModeratorSnapshotVisitor, ModeratorSnapshotVisitor>();

      container.Verify();

      var data = container.GetInstance<ISampleReceived>();
      var pipeline = container.GetInstance<IPipeline>();
      pipeline.Run(data);

      Console.ReadLine();
    }
  }
}
