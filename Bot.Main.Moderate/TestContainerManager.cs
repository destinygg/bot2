using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
using Bot.Tests;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using CoreTweet;
using CoreTweet.Streaming;
using NSubstitute;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using TwitchLib.Models.Client;

namespace Bot.Main.Moderate {
  public class TestContainerManager {
    public TestContainerManager(Action<Container> additionalRegistrations = null, Action<TestSettings> configureSettings = null, [CallerMemberName] string sqliteName = null) {
      Container = new Container();

      Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

      additionalRegistrations?.Invoke(Container);

      var settings = new TestSettings {
        SqlitePath = $"{sqliteName}_{TestHelper.RandomInt()}_Bot.sqlite"
      };
      configureSettings?.Invoke(settings);
      var settingsServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => settings, Container);
      Container.RegisterConditional(typeof(ISettings), settingsServiceRegistration, pc => !pc.Handled);

      Container.Register<IBotDbContext, BotDbContext>(Lifestyle.Scoped);
      Container.RegisterSingleton<IQueryCommandService<IBotDbContext>, QueryCommandService<IBotDbContext>>();
      Container.RegisterSingleton<IProvider<IBotDbContext>>(() => new DelegatedProvider<IBotDbContext>(() => Container.GetInstance<IBotDbContext>()));

      Container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
      Container.RegisterSingleton<IInMemoryRepository, InMemoryRepository>();
      Container.RegisterSingleton<IQueryCommandService<IUnitOfWork>, QueryCommandService<IUnitOfWork>>();
      Container.RegisterSingleton<IProvider<IUnitOfWork>>(() => new DelegatedProvider<IUnitOfWork>(() => Container.GetInstance<IUnitOfWork>()));

      Container.RegisterSingleton<IScopeCreator>(() => new DelegatedScopeCreator(() => AsyncScopedLifestyle.BeginScope(Container)));
      Container.RegisterDecorator(typeof(IQueryCommandService<>), typeof(ScopedQueryCommandServiceDecorator<>), Lifestyle.Singleton);

      Container.RegisterConditional<IProvider<IStreamStateService>, StreamStateServiceProvider>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterConditional<IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>, AegisPardonFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>, NukeMuteFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<IReceived<Moderator, IMessage>, Nuke>, NukeFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ICommandLogic, CommandLogic>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IModCommandLogic, ModCommandLogic>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IModCommandRepositoryLogic, ModCommandRepositoryLogic>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IModCommandRegex, ModCommandRegex>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IModCommandParser, ModCommandParser>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterConditional<IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, ModCommandFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, PunishmentFactory>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(PunishmentFactory)));
      Container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, RepositoryPunishmentFactory>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(RepositoryPunishmentFactory)));
      Container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, SelfSpamPunishmentFactory>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(SelfSpamPunishmentFactory)));
      Container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, LongSpamPunishmentFactory>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(LongSpamPunishmentFactory)));
      Container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, SingleLineSpamPunishmentFactory>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(SingleLineSpamPunishmentFactory)));
      Container.RegisterConditional<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, CommandFactory>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterConditional<IErrorableFactory<string, IReceived<IUser, ITransmittable>>, DestinyGgParser>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>, SnapshotFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>, SendableFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>, DestinyGgSerializer>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IPipelineManager, PipelineManager>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IClient, DestinyGgLoggingClient>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<ChatMessage, IReceived<IUser, ITransmittable>>, TwitchMessageParser>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterConditional(typeof(ILogger), c => typeof(Log4NetLogger<>).MakeGenericType(c.Consumer.ImplementationType), Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IPrivateConstants, PrivateConstants>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ITimeService, TimeService>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IDownloadMapper, DownloadMapper>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<string, string, string>, DownloadFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IErrorableFactory<string, string, string, string>, ErrorableDownloadFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IGenericClassFactory<string, string, string>, UrlXmlParser>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(UrlXmlParser)));
      Container.RegisterConditional<IGenericClassFactory<string, string, string>, UrlJsonParser>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(UrlJsonParser)));
      Container.RegisterConditional<IGenericClassFactory<string>, JsonParser>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(JsonParser)));

      Container.RegisterConditional<ICommandHandler, PeriodicTwitterStatusUpdater>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(PeriodicTwitterStatusUpdater)));
      Container.RegisterConditional<ICommandHandler, PeriodicClientChecker>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(PeriodicClientChecker)));
      Container.RegisterConditional<ICommandHandler, PeriodicStreamStatusUpdater>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(PeriodicStreamStatusUpdater)));
      Container.RegisterConditional<ICommandHandler, PeriodicMessages>(Lifestyle.Singleton, _notHandledAndMatchingClassName(nameof(PeriodicMessages)));
      Container.RegisterConditional<IFactory<TimeSpan, Action, Task>, PeriodicTaskFactory>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterConditional<IFactory<String>, LatestYoutubeFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<StreamingMessage, Status>, TwitterStatusFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<Status, string, IEnumerable<string>>, TwitterStatusFormatter>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ITwitterManager, TwitterManager>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ITwitterStreamingMessageObserver, TwitterStreamingMessageObserver>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterSingleton<ReceivedFactory>();

      Container.RegisterConditional<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>, SnapshotVisitor>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, CivilianPublicMessageToSendablesFactory>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterConditional<IReceivedVisitor<DelegatedSnapshotFactory>, ReceivedVisitor>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<ISendable<ITransmittable>, Moderator, ISendable<ITransmittable>>, PublicToPrivateMessageFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ISendableVisitor<string>, ConsoleSendableVisitor>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterDecorator(typeof(IFactory<>), typeof(FactoryTryCatchDecorator<>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IFactory<,>), typeof(FactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IFactory<,,>), typeof(FactoryTryCatchDecorator<,,>), Lifestyle.Singleton, p => p.ImplementationType.Name != nameof(DownloadFactory) && p.ImplementationType.Name != "IFactory`3");
      Container.RegisterDecorator(typeof(IFactory<,,,>), typeof(FactoryTryCatchDecorator<,,,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IProvider<>), typeof(CachedProviderDecorator<>), Lifestyle.Singleton, p => p.ImplementationType.Name == nameof(StreamStateServiceProvider));
      Container.RegisterDecorator(typeof(IErrorableFactory<,>), typeof(ErrorableFactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IErrorableFactory<,,>), typeof(ErrorableFactoryTryCatchDecorator<,,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IErrorableFactory<,,,>), typeof(ErrorableFactoryTryCatchDecorator<,,,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IGenericClassFactory<>), typeof(GenericClassFactoryTryCatchDecorator<>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IGenericClassFactory<,,>), typeof(GenericClassFactoryTryCatchDecorator<,,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(ICommandHandler), typeof(CommandHandlerTryCatchDecorator), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(ICommandHandler<>), typeof(CommandHandlerTryCatchDecorator<>), Lifestyle.Singleton);
    }

    public Container Container { get; }

    public Container InitializeAndIsolateRepository() {
      Container.GetInstance<RepositoryInitializer>().RecreateWithMasterData();
      return Container;
    }

    public static Container GetContainerWithRecreatedAndIsolatedDatabase([CallerMemberName] string sqlitePath = null) {

      var settings = Substitute.For<ISettings>();
      settings.SqlitePath.Returns(sqlitePath);
      Console.WriteLine("Database path is: " + sqlitePath);

      var containerManager = new TestContainerManager(
        container => {
          var settingsServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => settings, container);
          container.RegisterConditional(typeof(ISettings), settingsServiceRegistration, pc => !pc.Handled);
        });

      containerManager.Container.GetInstance<DatabaseInitializer>().Recreate();

      return containerManager.Container;
    }

    private Predicate<PredicateContext> _notHandledAndMatchingClassName(string className) => c => !c.Handled && c.Consumer.Target.Name == _toVariableName(className);

    private string _toVariableName(string className) => Char.ToLowerInvariant(className[0]) + className.Substring(1);
  }
}
