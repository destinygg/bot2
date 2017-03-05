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
using Bot.Tools.Logging;
using SimpleInjector;

namespace Bot.Main.Moderate {
  public class ContainerManager {
    private readonly Container _container;

    public ContainerManager() {
      _container = new Container();

      _container.RegisterSingleton<IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>, AegisPardonFactory>();
      _container.RegisterSingleton<IErrorableFactory<ParsedNuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>, NukeMuteFactory>();
      _container.RegisterSingleton<IFactory<IReceived<Moderator, IMessage>, ParsedNuke>, NukeFactory>();
      _container.RegisterSingleton<IModCommandLogic, ModCommandLogic>();
      _container.RegisterSingleton<IModCommandRegex, ModCommandRegex>();
      _container.RegisterSingleton<IModCommandParser, ModCommandParser>();

      _container.RegisterSingleton<IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, ModCommandFactory>();
      _container.RegisterSingleton<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, BanFactory>();
      _container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, CommandFactory>();

      _container.RegisterSingleton<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>, SnapshotFactory>();
      _container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>, SendableFactory>();
      _container.RegisterSingleton<ICommandHandler<IEnumerable<ISendable<ITransmittable>>>, Log4NetSender>();
      _container.RegisterSingleton<IPipeline, Pipeline.Pipeline>();

      _container.RegisterConditional(typeof(ILogger), c => typeof(Log4NetLogger<>).MakeGenericType(c.Consumer.ImplementationType), Lifestyle.Singleton, _ => true);
      _container.RegisterSingleton<ISettings, Settings>();
      _container.RegisterSingleton<ITimeService, TimeService>();

      _container.RegisterSingleton<IReceivedFactory, ReceivedFactory>();
      _container.RegisterSingleton<ISampleReceived, SampleReceived>();

      _container.RegisterSingleton<IReceivedVisitor<DelegatedSnapshotFactory>, ReceivedVisitor>();
      _container.RegisterSingleton<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>, SnapshotVisitor>();
      _container.RegisterSingleton<ISendableVisitor<string>, ConsoleSendableVisitor>();

      _container.RegisterDecorator(typeof(IErrorableFactory<,>), typeof(FactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(IErrorableFactory<,,>), typeof(FactoryTryCatchDecorator<,,>), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(ICommandHandler<>), typeof(CommandHandlerTryCatchDecorator<>), Lifestyle.Singleton);

      _container.Verify();
    }

    public IPipeline Pipeline => _container.GetInstance<Pipeline.Pipeline>();
    public ISampleReceived SampleReceived => _container.GetInstance<ISampleReceived>();
  }
}
