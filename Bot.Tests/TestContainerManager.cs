using System;
using System.Collections.Generic;
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
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace Bot.Tests {
  public class TestContainerManager {
    public TestContainerManager(Action<Container> additionalRegistrations = null) {
      Container = new Container();

      Container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();

      additionalRegistrations?.Invoke(Container);

      Container.Register<IBotDbContext, BotDbContext>(Lifestyle.Scoped);
      Container.RegisterSingleton<IDatabaseService<IBotDbContext>, DatabaseService<IBotDbContext>>();
      Container.RegisterSingleton<IScopeCreator>(() => new DelegatedScopeCreator(Container.BeginExecutionContextScope));
      Container.RegisterSingleton<IProvider<IBotDbContext>>(() => new DelegatedProvider<IBotDbContext>(() => Container.GetInstance<IBotDbContext>()));
      Container.RegisterDecorator(typeof(IDatabaseService<>), typeof(ScopedDatabaseServiceDecorator<>), Lifestyle.Singleton);

      Container.RegisterSingleton<IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<Pardon>>>, AegisLogic>();
      Container.RegisterSingleton<IErrorableFactory<IParsedNuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<Mute>>>, NukeLogic>();
      Container.RegisterSingleton<IModCommandLogic, ModCommandLogic>();
      Container.RegisterSingleton<IModCommandRegex, ModCommandRegex>();
      Container.RegisterSingleton<IModCommandParser, ModCommandParser>();

      Container.RegisterSingleton<IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, ModCommandFactory>();
      Container.RegisterSingleton<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>, BanFactory>();
      Container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>, CommandFactory>();

      Container.RegisterSingleton<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>, SnapshotFactory>();
      Container.RegisterSingleton<IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>, SendableFactory>();
      Container.RegisterSingleton<ICommandHandler<IEnumerable<ISendable<ITransmittable>>>, Log4NetSender>();
      Container.RegisterSingleton<IPipeline, Pipeline.Pipeline>();

      Container.RegisterConditional(typeof(ILogger), c => typeof(Log4NetLogger<>).MakeGenericType(c.Consumer.ImplementationType), Lifestyle.Singleton, _ => true);
      Container.RegisterConditional<ISettings, Settings>(Lifestyle.Singleton, c => !c.Handled);
      Container.RegisterConditional<ITimeService, TimeService>(Lifestyle.Singleton, c => !c.Handled);

      Container.RegisterSingleton<IReceivedFactory, ReceivedFactory>();
      Container.RegisterSingleton<ISampleReceived, SampleReceived>();

      Container.RegisterSingleton<IReceivedVisitor<DelegatedSnapshotFactory>, ReceivedVisitor>();
      Container.RegisterSingleton<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>, SnapshotVisitor>();
      Container.RegisterSingleton<ISendableVisitor<string>, ConsoleSendableVisitor>();

      Container.RegisterDecorator(typeof(IErrorableFactory<,>), typeof(FactoryTryCatchDecorator<,>), Lifestyle.Singleton);
      Container.RegisterDecorator(typeof(ICommandHandler<>), typeof(CommandHandlerTryCatchDecorator<>), Lifestyle.Singleton);

      Container.Verify();
    }

    public Container Container { get; }

  }
}
