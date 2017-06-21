using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Database;
using Bot.Database.Interfaces;
using Bot.Logic;
using Bot.Logic.Interfaces;
using Bot.Logic.ReceivedVisitor;
using Bot.Logic.SendableVisitor;
using Bot.Logic.SnapshotVisitor;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Pipeline;
using Bot.Pipeline.Interfaces;
using Bot.Repository;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using CoreTweet;
using CoreTweet.Streaming;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using TwitchLib.Models.Client;

namespace Bot.Main.Moderate {
  public class ContainerManager {
    private readonly Container _container;

    public ContainerManager() {
      _container = new Container();

      _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

      _container.Register<IBotDbContext, BotDbContext>(Lifestyle.Scoped);
      _container.RegisterSingleton<IQueryCommandService<IBotDbContext>, QueryCommandService<IBotDbContext>>();
      _container.RegisterSingleton<IProvider<IBotDbContext>>(() => new DelegatedProvider<IBotDbContext>(() => _container.GetInstance<IBotDbContext>()));

      _container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
      _container.RegisterSingleton<IInMemoryRepository, InMemoryRepository>();
      _container.RegisterSingleton<IQueryCommandService<IUnitOfWork>, QueryCommandService<IUnitOfWork>>();
      _container.RegisterSingleton<IProvider<IUnitOfWork>>(() => new DelegatedProvider<IUnitOfWork>(() => _container.GetInstance<IUnitOfWork>()));

      _container.RegisterSingleton<IProvider<IStreamStateService>, StreamStateServiceProvider>();

      _container.RegisterSingleton<IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>, AegisPardonFactory>();
      _container.RegisterSingleton<IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>, NukeMuteFactory>();
      _container.RegisterSingleton<IFactory<IReceived<Moderator, IMessage>, Nuke>, NukeFactory>();
      _container.RegisterSingleton<ICommandLogic, CommandLogic>();
      _container.RegisterSingleton<IModCommandLogic, ModCommandLogic>();
      _container.RegisterSingleton<IModCommandRepositoryLogic, ModCommandRepositoryLogic>();
      _container.RegisterSingleton<IModCommandRegex, ModCommandRegex>();
      _container.RegisterSingleton<IModCommandParser, ModCommandParser>();

      _container.RegisterSingleton<IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, ModCommandFactory>();
      _container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, PunishmentFactory>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "punishmentFactory");
      _container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, RepositoryPunishmentFactory>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "repositoryPunishmentFactory");
      _container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, SelfSpamPunishmentFactory>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "selfSpamPunishmentFactory");
      _container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, LongSpamPunishmentFactory>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "longSpamPunishmentFactory");
      _container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, SingleLineSpamPunishmentFactory>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "singleLineSpamPunishmentFactory");
      _container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, CommandFactory>();

      _container.RegisterSingleton<IErrorableFactory<string, IReceived<IUser, ITransmittable>>, DestinyGgParser>();
      _container.RegisterSingleton<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>, SnapshotFactory>();
      _container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>, SendableFactory>();
      _container.RegisterSingleton<IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>, DestinyGgSerializer>();
      _container.RegisterSingleton<IPipelineManager, PipelineManager>();
      _container.RegisterSingleton<IClient, DestinyGgLoggingClient>();
      _container.RegisterSingleton<IFactory<ChatMessage, IReceived<IUser, ITransmittable>>, TwitchMessageParser>();

      _container.RegisterConditional(typeof(ILogger), c => typeof(Log4NetLogger<>).MakeGenericType(c.Consumer.ImplementationType), Lifestyle.Singleton, _ => true);
      _container.RegisterSingleton<ISettings, Settings>();
      _container.RegisterSingleton<IPrivateConstants, PrivateConstants>();
      _container.RegisterSingleton<ITimeService, TimeService>();
      _container.RegisterSingleton<IDownloadMapper, DownloadMapper>();
      _container.RegisterSingleton<IFactory<string, string, string>, DownloadFactory>();
      _container.RegisterSingleton<IErrorableFactory<string, string, string, string>, ErrorableDownloadFactory>();
      _container.RegisterConditional<IGenericClassFactory<string, string, string>, UrlXmlParser>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "urlXmlParser");
      _container.RegisterConditional<IGenericClassFactory<string, string, string>, UrlJsonParser>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "urlJsonParser");
      _container.RegisterConditional<IGenericClassFactory<string>, JsonParser>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "jsonParser");

      _container.RegisterConditional<ICommandHandler, PeriodicClientChecker>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "periodicClientChecker");
      _container.RegisterConditional<ICommandHandler, PeriodicStreamStatusUpdater>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "periodicStreamStatusUpdater");
      _container.RegisterConditional<ICommandHandler, PeriodicMessages>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "periodicMessages");
      _container.RegisterSingleton<IFactory<TimeSpan, Action, Task>, PeriodicTaskFactory>();

      _container.RegisterSingleton<IFactory<StreamingMessage, Status>, TwitterStatusFactory>();
      _container.RegisterSingleton<IFactory<Status, string, IEnumerable<string>>, TwitterStatusFormatter>();
      _container.RegisterSingleton<ITwitterManager, TwitterManager>();
      _container.RegisterSingleton<ITwitterStreamingMessageObserver, TwitterStreamingMessageObserver>();

      _container.RegisterSingleton<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>, SnapshotVisitor>();
      _container.RegisterSingleton<IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, CivilianPublicMessageToSendablesFactory>();

      _container.RegisterSingleton<IReceivedVisitor<DelegatedSnapshotFactory>, ReceivedVisitor>();
      _container.RegisterSingleton<IFactory<ISendable<ITransmittable>, Moderator, ISendable<ITransmittable>>, PublicToPrivateMessageFactory>();
      _container.RegisterSingleton<ISendableVisitor<string>, ConsoleSendableVisitor>();

      _container.RegisterDecorator(typeof(IFactory<>), typeof(FactoryTryCatchDecorator<>), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(IFactory<,>), typeof(FactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(IFactory<,,>), typeof(FactoryTryCatchDecorator<,,>), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(IProvider<>), typeof(CachedProviderDecorator<>), Lifestyle.Singleton, p => p.ImplementationType.Name == nameof(StreamStateServiceProvider));
      _container.RegisterDecorator(typeof(IErrorableFactory<,>), typeof(ErrorableFactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(IErrorableFactory<,,>), typeof(ErrorableFactoryTryCatchDecorator<,,>), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(IGenericClassFactory<>), typeof(GenericClassFactoryTryCatchDecorator<>), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(IGenericClassFactory<,,>), typeof(GenericClassFactoryTryCatchDecorator<,,>), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(ICommandHandler), typeof(CommandHandlerTryCatchDecorator), Lifestyle.Singleton);
      _container.RegisterDecorator(typeof(ICommandHandler<>), typeof(CommandHandlerTryCatchDecorator<>), Lifestyle.Singleton);

      _container.Verify();
    }

    public IPipelineManager PipelineManager => _container.GetInstance<IPipelineManager>();
  }
}
