﻿using System.Collections.Generic;
using Bot.Database;
using Bot.Logic;
using Bot.Logic.Interfaces;
using Bot.Logic.ReceivedVisitor;
using Bot.Logic.SendableVisitor;
using Bot.Logic.SnapshotVisitor;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Pipeline;
using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using SimpleInjector;

namespace Bot.Tests {
  public class ContainerManager {
    public ContainerManager() {
      Container = new Container();

      Container.Register<DatabaseInitializer>();
      Container.Register<BotDbContextManager>();

      Container.RegisterSingleton<INukeLogic, NukeLogic>();
      Container.RegisterSingleton<IModCommandLogic, ModCommandLogic>();
      Container.RegisterSingleton<IModCommandRegex, ModCommandRegex>();
      Container.RegisterSingleton<IModCommandParser, ModCommandParser>();

      Container.RegisterSingleton<IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, ModCommandFactory>();
      Container.RegisterSingleton<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, BanFactory>();
      Container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, CommandFactory>();

      Container.RegisterSingleton<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>, SnapshotFactory>();
      Container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>, SendableFactory>();
      Container.RegisterSingleton<ICommandHandler<IEnumerable<ISendable<ITransmittable>>>, ConsoleSender>();
      Container.RegisterSingleton<IPipeline, Pipeline.Pipeline>();

      Container.RegisterSingleton<ILogger, Logger>();
      Container.RegisterSingleton<ILogFormatter, LogFormatter>();
      Container.RegisterSingleton<ILogPersister, ConsolePersister>();

      Container.RegisterSingleton<ITimeService, TimeService>();
      Container.RegisterSingleton<IReceivedFactory, ReceivedFactory>();
      Container.RegisterSingleton<ISampleReceived, SampleReceived>();

      Container.RegisterSingleton<IUserVisitor<IReceivedVisitor<DelegatedSnapshotFactory>>, Logic.ReceivedVisitor.UserVisitor>();
      Container.RegisterSingleton<ModeratorReceivedVisitor, ModeratorReceivedVisitor>();
      Container.RegisterSingleton<CivilianReceivedVisitor, CivilianReceivedVisitor>();

      Container.RegisterSingleton<IUserVisitor<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>>, Logic.SnapshotVisitor.UserVisitor>();
      Container.RegisterSingleton<CivilianSnapshotVisitor, CivilianSnapshotVisitor>();
      Container.RegisterSingleton<ModeratorSnapshotVisitor, ModeratorSnapshotVisitor>();

      Container.RegisterSingleton<ISendableVisitor<string>, ConsoleSendableVisitor>();

      Container.RegisterDecorator(typeof(IErrorableFactory<,>), typeof(FactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(ICommandHandler<>), typeof(CommandHandlerTryCatchDecorator<>), Lifestyle.Singleton);
      //container.RegisterDecorator<ICommandHandler<IEnumerable<ISendable<ITransmittable>>>, CommandHandlerTryCatchDecorator<IEnumerable<ISendable<ITransmittable>>>>(Lifestyle.Singleton);

      Container.Verify();
    }

    public Container Container { get; }

  }
}
