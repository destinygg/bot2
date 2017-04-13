using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
using NSubstitute;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Bot.Tests {
  public class TestContainerManager {
    public TestContainerManager(Action<Container> additionalRegistrations = null) {
      Container = new Container();

      Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

      additionalRegistrations?.Invoke(Container);

      Container.Register<IBotDbContext, BotDbContext>(Lifestyle.Scoped);
      Container.RegisterSingleton<IQueryCommandService<IBotDbContext>, QueryCommandService<IBotDbContext>>();
      Container.RegisterSingleton<IProvider<IBotDbContext>>(() => new DelegatedProvider<IBotDbContext>(() => Container.GetInstance<IBotDbContext>()));

      Container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
      Container.RegisterSingleton<IQueryCommandService<IUnitOfWork>, QueryCommandService<IUnitOfWork>>();
      Container.RegisterSingleton<IProvider<IUnitOfWork>>(() => new DelegatedProvider<IUnitOfWork>(() => Container.GetInstance<IUnitOfWork>()));

      Container.RegisterSingleton<IScopeCreator>(() => new DelegatedScopeCreator(() => AsyncScopedLifestyle.BeginScope(Container)));
      Container.RegisterDecorator(typeof(IQueryCommandService<>), typeof(ScopedQueryCommandServiceDecorator<>), Lifestyle.Singleton);

      Container.RegisterSingleton<IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>, AegisPardonFactory>();
      Container.RegisterSingleton<IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>, NukeMuteFactory>();
      Container.RegisterSingleton<IFactory<IReceived<Moderator, IMessage>, Nuke>, NukeFactory>();
      Container.RegisterSingleton<IModCommandLogic, ModCommandLogic>();
      Container.RegisterConditional<IModCommandRegex, ModCommandRegex>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterSingleton<IModCommandParser, ModCommandParser>();

      Container.RegisterSingleton<IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, ModCommandFactory>();
      Container.RegisterSingleton<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, BanFactory>();
      Container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, CommandFactory>();

      Container.RegisterSingleton<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>, SnapshotFactory>();
      Container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>, SendableFactory>();
      Container.RegisterSingleton<ICommandHandler<IEnumerable<ISendable<ITransmittable>>>, Log4NetSender>();
      Container.RegisterSingleton<IPipeline, Pipeline.Pipeline>();

      Container.RegisterConditional(typeof(ILogger), c => typeof(Log4NetLogger<>).MakeGenericType(c.Consumer.ImplementationType), Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ISettings, Settings>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ITimeService, TimeService>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterSingleton<ReceivedFactory>();

      Container.RegisterSingleton<IReceivedVisitor<DelegatedSnapshotFactory>, ReceivedVisitor>();
      Container.RegisterSingleton<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>, SnapshotVisitor>();
      Container.RegisterSingleton<ISendableVisitor<string>, ConsoleSendableVisitor>();

      Container.RegisterDecorator(typeof(IErrorableFactory<,>), typeof(FactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(IErrorableFactory<,,>), typeof(FactoryTryCatchDecorator<,,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(ICommandHandler<>), typeof(CommandHandlerTryCatchDecorator<>), Lifestyle.Singleton);
    }

    public Container Container { get; }

    public Container InitializeAndIsolateRepository(Action<ISettings> configureSettings = null, [CallerMemberName] string sqliteName = null) {

      var settings = Substitute.For<ISettings>();
      settings.SqlitePath.Returns($"{sqliteName}_{TestHelper.RandomInt()}_Bot.sqlite");
      configureSettings?.Invoke(settings);
      var containerManager = new TestContainerManager(
        container => {
          var settingsServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => settings, container);
          container.RegisterConditional(typeof(ISettings), settingsServiceRegistration, pc => !pc.Handled);
        });

      containerManager.Container.GetInstance<RepositoryInitializer>().RecreateWithMasterData();

      return containerManager.Container;
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
