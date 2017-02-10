using System.Collections.Generic;
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

namespace Bot.Main.Moderate {
  public class ContainerManager {
    private readonly Container _container;

    public ContainerManager() {
      _container = new Container();

      _container.RegisterSingleton<INukeLogic, NukeLogic>();
      _container.RegisterSingleton<IModCommandLogic, ModCommandLogic>();
      _container.RegisterSingleton<IModCommandRegex, ModCommandRegex>();
      _container.RegisterSingleton<IModCommandParser, ModCommandParser>();

      _container.RegisterSingleton<IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, ModCommandFactory>();
      _container.RegisterSingleton<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, BanFactory>();
      _container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, CommandFactory>();

      _container.RegisterSingleton<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>, SnapshotFactory>();
      _container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>, SendableFactory>();
      _container.RegisterSingleton<ICommandHandler<IEnumerable<ISendable<ITransmittable>>>, ConsoleSender>();
      _container.RegisterSingleton<IPipeline, Pipeline.Pipeline>();

      _container.RegisterSingleton<ILogger, Logger>();
      _container.RegisterSingleton<ILogFormatter, LogFormatter>();
      _container.RegisterSingleton<ILogPersister, ConsolePersister>();

      _container.RegisterSingleton<ITimeService, TimeService>();
      _container.RegisterSingleton<IReceivedFactory, ReceivedFactory>();
      _container.RegisterSingleton<ISampleReceived, SampleReceived>();

      _container.RegisterSingleton<IUserVisitor<IReceivedVisitor<DelegatedSnapshotFactory>>, Logic.ReceivedVisitor.UserVisitor>();
      _container.RegisterSingleton<ModeratorReceivedVisitor, ModeratorReceivedVisitor>();
      _container.RegisterSingleton<CivilianReceivedVisitor, CivilianReceivedVisitor>();

      _container.RegisterSingleton<IUserVisitor<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>>, Logic.SnapshotVisitor.UserVisitor>();
      _container.RegisterSingleton<CivilianSnapshotVisitor, CivilianSnapshotVisitor>();
      _container.RegisterSingleton<ModeratorSnapshotVisitor, ModeratorSnapshotVisitor>();

      _container.RegisterSingleton<ISendableVisitor<string>, ConsoleSendableVisitor>();

      _container.RegisterDecorator(typeof(IErrorableFactory<,>), typeof(FactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(ICommandHandler<>), typeof(CommandHandlerTryCatchDecorator<>), Lifestyle.Singleton);
      //container.RegisterDecorator<ICommandHandler<IEnumerable<ISendable<ITransmittable>>>, CommandHandlerTryCatchDecorator<IEnumerable<ISendable<ITransmittable>>>>(Lifestyle.Singleton);

      _container.Verify();
    }

    public IPipeline Pipeline => _container.GetInstance<Pipeline.Pipeline>();
    public ISampleReceived SampleReceived => _container.GetInstance<ISampleReceived>();
  }
}
