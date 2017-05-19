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
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using CoreTweet;
using CoreTweet.Streaming;
using NSubstitute;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Bot.Tests {
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

      Container.RegisterConditional<IStreamStateService, StreamStateService>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterConditional<IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>, AegisPardonFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>, NukeMuteFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<IReceived<Moderator, IMessage>, Nuke>, NukeFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ICommandLogic, CommandLogic>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IModCommandLogic, ModCommandLogic>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IModCommandRegex, ModCommandRegex>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IModCommandParser, ModCommandParser>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterConditional<IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, ModCommandFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, PunishmentFactory>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "punishmentFactory" && !c.Handled);
      Container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, RepositoryPunishmentFactory>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "repositoryPunishmentFactory" && !c.Handled);
      Container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, SelfSpamPunishmentFactory>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "selfSpamPunishmentFactory" && !c.Handled);
      Container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, LongSpamPunishmentFactory>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "longSpamPunishmentFactory" && !c.Handled);
      Container.RegisterConditional<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, SingleLineSpamPunishmentFactory>(Lifestyle.Singleton, c => c.Consumer.Target.Name == "singleLineSpamPunishmentFactory" && !c.Handled);
      Container.RegisterConditional<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, CommandFactory>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterConditional<IErrorableFactory<string, IReceived<IUser, ITransmittable>>, DestinyGgParser>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>, SnapshotFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>, SendableFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>>, DestinyGgSerializer>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IPipeline, PipelineManager>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IClient, DestinyGgLoggingClient>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterConditional(typeof(ILogger), c => typeof(Log4NetLogger<>).MakeGenericType(c.Consumer.ImplementationType), Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IPrivateConstants, PrivateConstants>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ITimeService, TimeService>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IDownloadMapper, DownloadMapper>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<TimeSpan, Action, Task>, PeriodicTaskFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IErrorableFactory<string, string, string, string>, ErrorableDownloadFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IGenericClassFactory<string, string, string>, UrlXmlParser>(Lifestyle.Singleton, c => !c.Handled && c.Consumer.Target.Name == "urlXmlParser");
      Container.RegisterConditional<IGenericClassFactory<string, string, string>, UrlJsonParser>(Lifestyle.Singleton, c => !c.Handled && c.Consumer.Target.Name == "urlJsonParser");
      Container.RegisterConditional<IGenericClassFactory<string>, JsonParser>(Lifestyle.Singleton, c => !c.Handled && c.Consumer.Target.Name == "jsonParser");

      Container.RegisterConditional<IFactory<StreamingMessage, Status>, TwitterStatusFactory>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<IFactory<Status, string, IEnumerable<string>>, TwitterStatusFormatter>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ITwitterManager, TwitterManager>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ITwitterStreamingMessageObserver, TwitterStreamingMessageObserver>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterSingleton<ReceivedFactory>();

      Container.RegisterConditional<IReceivedVisitor<DelegatedSnapshotFactory>, ReceivedVisitor>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>, SnapshotVisitor>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ISendableVisitor<string>, ConsoleSendableVisitor>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterDecorator(typeof(IFactory<,>), typeof(FactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IFactory<,,>), typeof(FactoryTryCatchDecorator<,,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IErrorableFactory<,>), typeof(ErrorableFactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IErrorableFactory<,,>), typeof(ErrorableFactoryTryCatchDecorator<,,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IGenericClassFactory<>), typeof(GenericClassFactoryTryCatchDecorator<>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IGenericClassFactory<,,>), typeof(GenericClassFactoryTryCatchDecorator<,,>), Lifestyle.Singleton);
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

  }
}
