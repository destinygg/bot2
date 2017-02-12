using System;
using Bot.Database.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Database {
  public class DatabaseService<TContext> : IDatabaseService<TContext>
    where TContext : IDisposable, ISavable {

    private readonly IProvider<TContext> _contextProvider;

    public DatabaseService(IProvider<TContext> contextProvider) {
      _contextProvider = contextProvider;
    }

    TResult IDatabaseService<TContext>.Query<TResult>(Func<TContext, TResult> query) =>
      _execute(query);

    int IDatabaseService<TContext>.Command(Func<TContext, int> command) =>
      _execute(command);

    private TResult _execute<TResult>(Func<TContext, TResult> body) =>
      _contextProvider.Get().Apply(body);

  }
}
